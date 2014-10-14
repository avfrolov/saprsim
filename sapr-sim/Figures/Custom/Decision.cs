using sapr_sim.Figures.Basic;
using sapr_sim.Figures.Basic.Enums;
using sapr_sim.Figures.Custom.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.Figures.Custom
{
    // повесим на класс болшьшоеее TODO...
    [Serializable]
    public class Decision : ComplexFigure, SchemaElement
    {
        //видимые порты сущностей (индексы в списке фигур)
        private int t_id = -1, b_id = -1, r_id = -1;
        private Rhomb bound;
        private Label label;
        private Port entityPort;

        public Decision()
        {
            bound = new Rhomb();
            bound.Text = "";
            bound.Size = new SizeF(36F, 36F);
            bound.TextChangeEnable = false;
            bound.FigureBrush = new LinearGradientBrush(
               new PointF(-bound.Size.Width / 2, 10),
               new PointF(bound.Size.Width / 2, 10),
               Color.LemonChiffon,
               Color.FromArgb(255, 255, 250, 250));
            primitives.Add(bound);

            MainFigureIndex = 0;

            label = new Label();
            label.OwnerFigure = this;
            label.Text = "Принятие решения";
            primitives.Add(label);

            //пересчет положения ромба относительно надписи 1
            bound.Location = new PointF(label.Location.X - (int)label.Size.Width / 2, 
                primitives[1].Location.Y + (int)label.Size.Height / 2 + 3 * defaultLabelOffset + (int)bound.Size.Height / 2);

            //входной порт сущностей
            entityPort = new Port(primitives[MainFigureIndex], this, ConnectorDirection.Input, ConnectorType.Entity);
            primitives.Add(entityPort);

            Label yesLabel = new Label("[Да]");
            yesLabel.OwnerFigure = this;
            yesLabel.Location = new PointF(bound.Location.X + 20, bound.Location.Y + 20);
            primitives.Add(yesLabel);

            Label noLabel = new Label("[Нет]");
            noLabel.OwnerFigure = this;
            noLabel.Location = new PointF(bound.Location.X + 20, bound.Location.Y - 20);
            primitives.Add(noLabel);

            TopPortVisible = true;
            RightPortVisible = true;
            BottomPortVisible = true;

            RecalcFigure();
        }
        public override void RecalcFigure()
        {
            //пересчет положения входного порта
            entityPort.Location = new PointF(bound.Location.X - (int)bound.Size.Width / 2 - defaultPortOffset, bound.Location.Y);

            //выходные порты сущностей
            if (r_id > 0 && r_id < primitives.Count)
            {
                primitives[r_id].Location = new PointF(bound.Location.X + (int)bound.Size.Width / 2, bound.Location.Y);
            }
            if (t_id > 0 && r_id < primitives.Count)
            {
                primitives[t_id].Location = new PointF(bound.Location.X, bound.Location.Y - (int)bound.Size.Height / 2);
            }
            if (b_id > 0 && r_id < primitives.Count)
            {
                primitives[b_id].Location = new PointF(bound.Location.X, bound.Location.Y + (int)bound.Size.Height / 2);
            }
        }

        /// <summary>
        /// Определяет направление порта (a.frolov: надо бы переделать на enum в классе Port)
        /// </summary>
        /// <param name="port">Порт, расположение которого требуется определить</param>
        /// <returns>0 - порт верхний; 1 - порт средний (правый); 2 - порт нижний; -1 - не принадлежит фигуре</returns>
        public int FindPortDirection(Port port)
        {
            if (port == primitives[t_id])
                return 0;
            if (port == primitives[r_id])
                return 1;
            if (port == primitives[b_id])
                return 2;
            return -1;
        }

        //обработка добавления и удаления портов
        public bool TopPortVisible
        {
            get { return t_id > 0 ; }
            set
            {
                if (value && t_id < 0)
                {
                    //добавляем верхний порт и соотв. линию
                    primitives.Add(new Port(bound, this, ConnectorDirection.Output, ConnectorType.Entity, ConnectorOrientation.BottomToTop));                    
                    t_id = primitives.Count - 1;
                    (primitives[t_id] as Port).CheckConnection = this.CheckConnection;
                }
                else if (!value && t_id > 0)
                {
                    //удаляем верхний порт и линию
                    primitives.RemoveAt(t_id);
                    RecalcIndexes(t_id);
                    t_id = -1;
                }
            }
        }

        //нижний порт блока решения
        public bool BottomPortVisible
        {
            get { return b_id > 0; }
            set
            {
                if (value && b_id < 0)
                {
                    //добавляем нижний порт и соотв. линию
                    primitives.Add(new Port(bound, this, ConnectorDirection.Output, ConnectorType.Entity, ConnectorOrientation.TopToBottom));
                    b_id = primitives.Count - 1;
                    (primitives[b_id] as Port).CheckConnection = this.CheckConnection;
                }
                else if (!value && b_id > 0)
                {
                    //удаляем нижний порт и линию
                    primitives.RemoveAt(b_id);
                    RecalcIndexes(b_id);
                    b_id = -1;
                }
            }
        }

        //правый порт блока решения
        public bool RightPortVisible
        {
            get { return r_id > 0; }
            set
            {
                if (value && r_id < 0)
                {
                    primitives.Add(new Port(bound, this, ConnectorDirection.Output, ConnectorType.Entity));
                    r_id = primitives.Count - 1;
                    (primitives[r_id] as Port).CheckConnection = this.CheckConnection;
                }
                else if (!value && r_id > 0)
                {
                    primitives.RemoveAt(r_id);
                    RecalcIndexes(r_id);
                    r_id = -1;
                }
            }
        }

        protected void RecalcIndexes(int id)
        {
            if (r_id > id) r_id--;
            if (b_id > id) b_id--;
            if (t_id > id) t_id--;
        }

        public override bool CheckConnection()
        {
            //разрешаем подключать только 2 линии
            int connectionCount = 0;
            //порты имеют индексы: 5, 6, 7
            //сколько выходных портов подключено?
            for (int i = 5; i < primitives.Count; i++)
                if ((primitives[i] as IConnectable).IsConnected)
                    connectionCount++;
            //если 2 вых. порта уже подключены - блокируем оставшийся порт
            return connectionCount != 3;
        }
    }
}
