using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using System.ComponentModel;

using sapr_sim.Figures.Basic.Lines;
using sapr_sim.Figures.Basic;
using sapr_sim.Figures.Basic.Enums;
using sapr_sim.Figures.Custom.Enum;
using sapr_sim.Exceptions;

namespace sapr_sim.Figures.Basic.Markers
{
    public partial class EndLineMarker : Marker, IConnectable
    {
        int pointIndex;
        ConnectorType type;
        Diagram diagram;
        public static ConnectBlocksDelegate ConnectBlocks;
        public static DisconnectBlocksDelegate DisconnectBlocks;

        public EndLineMarker(Diagram diagram, int pointIndex)
        {
            this.diagram = diagram;
            this.pointIndex = pointIndex;
        }

        public override void UpdateLocation()
        {
            Line line = (targetFigure as Line);
            //if (line.From == null || line.To == null)
            //    return;
            // {
            //если у линии оторванный конец, то сажаем на него маркер
            if (line.From == null && pointIndex == 0)
            {
                line.From = this;
                location = new PointF(line.Path.PathPoints[0].X, line.Path.PathPoints[0].Y);
                return;
            }
            else if (line.To == null && pointIndex == 1)
            {
                line.To = this;
                location = new PointF(line.Path.PathPoints[line.Path.PointCount - 1].X,
                    line.Path.PathPoints[line.Path.PointCount - 1].Y);
                return;
            }

            //}
            //фигура, с которой связана линия
            IConnectable figure = pointIndex == 0 ? line.From : line.To;
            location = figure.Location;
        }
        public override void Draw(Graphics gr)
        {
            gr.DrawRectangle(Pens.Black, location.X - defaultSize, location.Y - defaultSize, defaultSize * 2, defaultSize * 2);
            gr.FillRectangle(Brushes.Red, location.X - defaultSize, location.Y - defaultSize, defaultSize * 2, defaultSize * 2);
        }
        public override void Offset(float dx, float dy)
        {
            base.Offset(dx, dy);
            //линия маркера
            Line line = (targetFigure as Line);
            //ищем фигуру под маркером
            SolidFigure figure = null;
            for (int i = diagram.Figures.Count - 1; i >= 0; i--)
            {
                if (diagram.Figures[i].IsInsidePoint(new Point((int)location.X, (int)location.Y)))
                {
                    if (diagram.Figures[i] is IConnectable)
                    {
                        figure = (SolidFigure)diagram.Figures[i];
                        break;
                    }
                    else if (diagram.Figures[i] is ComplexFigure)
                    {
                        //ищем порт комплексной фигуры
                        foreach (SolidFigure SF in (diagram.Figures[i] as ComplexFigure).SolidFigures)
                        {
                            if (SF.IsInsidePoint(new Point((int)location.X, (int)location.Y))
                                && (SF is IConnectable))
                            {
                                if ((SF as IConnectable).EnableMultipleConnection
                                    || !(SF as IConnectable).IsConnected
                                    || ((SF as IConnectable).IsConnected
                                    && ((SF == line.To) || (SF == line.From))
                                    ))
                                    figure = SF;
                                else
                                    throw new ConnectorException("PortMultipleConnectionException");
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            if (figure == null)
                figure = this;//если под маркером нет фигуры, то просто коннектим линию к самому маркеру


            //не позволяем конектится самому к себе
            if (line.From == figure || line.To == figure)
                return;
            //включаем фигуру
            if (figure is IConnectable)
                (figure as IConnectable).Connect();
            //Осуществление встроенной проверки на только что подлюченную фигуру
            if (!(figure as IConnectable).IsConnectionOK())
            {
                throw new ConnectorException("Нарушение логики подлючения коннектора!");
            }
            string old_from_name = "";
            string old_to_name = "";
            //отключаем от старый коннектор
            //обновляем коннекторы линии
            if (pointIndex == 0)
            {
                if (line.From != figure && line.From != null && !(line.From is Marker))
                {
                    (line.From as IConnectable).Disconnect();
                    old_from_name = line.From.OwnerFigure.Name;
                }
                line.From = (figure as IConnectable);
            }
            else
            {
                if (line.To != figure && line.To != null && !(line.To is Marker))
                {
                    (line.To as IConnectable).Disconnect();
                    old_to_name = line.To.OwnerFigure.Name;
                }
                line.To = (figure as IConnectable);
            }
            //Изменение стиля линий
            if (line.From.OwnerFigure != null)
            {
                line.FigurePen = (line.From.OwnerFigure as ISolidFigure).FigurePen;
                //если линия сущностей, то делаем жирной
                if (line.From.Type == ConnectorType.Entity)
                    line.FigurePen = new Pen(line.FigurePen.Color, 2);
                else
                    line.FigurePen = new Pen(line.FigurePen.Color, 1);
                //окрасим порт назначения
                if (line.To != null)
                {
                    (line.To as SolidFigure).FigurePen = line.FigurePen;
                    (line.To as SolidFigure).FigureBrush = new SolidBrush(line.FigurePen.Color);
                }
            }
            //Проверка на правильность соединения портов, которая работает,
            //когда линия подключена к фигурам с обоих концов (а не 2м маркерам / фигуре и маркеру)
            if ((line.From is IConnectable && line.To is IConnectable)
                && !(line.From is EndLineMarker)
                && !(line.To is EndLineMarker))
            {
                //если ошибка - отцепляем линию и приклеиваем к маркеру
                if (line.From.MainFigure == line.To.MainFigure)
                {
                    if (pointIndex == 0) line.From = this; else line.To = this;
                    throw new ConnectorException("SameBlockConnectorException");
                }
                if (line.From.Type != line.To.Type)
                {
                    if (pointIndex == 0) line.From = this; else line.To = this;
                    throw new ConnectorException("PortTypeException");
                }
                if (line.From.Direction == line.To.Direction)
                {
                    if (pointIndex == 0) line.From = this; else line.To = this;
                    throw new ConnectorException("PortDirectionException");
                }
                //если проверка прошла и направление линии некорректное,
                //то разворачиваем линию по направлению портов Output - Input
                if (line.From.Direction == ConnectorDirection.Input)
                {
                    IConnectable tmp;
                    tmp = line.From;
                    line.From = line.To;
                    line.To = tmp;
                }
            }
            //все проверки прошли, линия корректна. Выполняем отсоединение/соединение блоков логики
            if (old_from_name != "" && line.To != null)
                if (line.To.OwnerFigure != null)
                    EndLineMarker.DisconnectBlocks(old_from_name, line.To.OwnerFigure.Name);
            if (old_to_name != "" && line.From != null)
                if (line.From.OwnerFigure != null)
                    EndLineMarker.DisconnectBlocks(line.From.OwnerFigure.Name, old_to_name);
            int param = 0;
            // incorrect checking
            //if ((old_to_name == "" || old_from_name == "") && line.From != null && line.To != null)
            //    if (line.From.OwnerFigure != null && line.To.OwnerFigure != null)
            //    {
            //        if (line.From.OwnerFigure is DecisionBlock)
            //            param = (line.From.OwnerFigure as DecisionBlock).FindPortDirection(line.From as PortFigure);
            //        EndLineMarker.ConnectBlocks(line.From.OwnerFigure.Name, line.To.OwnerFigure.Name, param);
            //    }
        }
        public void Connect() { }
        public void Disconnect() { }
        public bool EnableMultipleConnection
        {
            get { return false; }
            set { }
        }
        public bool IsConnected
        {
            get { return true; }
            set { }
        }
        public float X
        {
            get { return location.X; }
            set { location.X = value; }
        }
        public float Y
        {
            get { return location.Y; }
            set { location.Y = value; }
        }
        public ConnectorOrientation Orientation
        {
            get { return ConnectorOrientation.LeftToRight; }
            set { }
        }
        public ConnectorDirection Direction
        {
            get { return (pointIndex == 0 ? ConnectorDirection.Output : ConnectorDirection.Input); }
            set { }
        }
        public Figure MainFigure
        {
            get { return null; }
            set { }
        }
        public ConnectorType Type
        {
            get { return (ConnectorType)Enum.GetValues(type.GetType()).GetValue(0); }
            set { }
        }

        //TODO - хз что это
        public Figure OwnerFigure
        {
            get { return null; }
            set { }
        }
        public bool IsConnectionOK()
        {
            return true;
        }

        //public CheckConnectionDelegate CheckConnection
        //{
        //    set { }
        //    get { return null; }
        //}
    }
}
