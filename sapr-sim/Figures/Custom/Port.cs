using sapr_sim.Figures.Basic;
using sapr_sim.Figures.Basic.Enums;
using sapr_sim.Figures.Basic.Markers;
using sapr_sim.Figures.Custom.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.Figures.Custom
{
    [Serializable]
    public class Port : SolidFigure, SchemaElement, IConnectable
    {
        private new int defaultSize = 3;
        private CheckConnectionDelegate checkConnection = null;
        private Figure mainFigure;
        private bool isConnected;
        private bool enableMultipleConnection = false;
        private ConnectorDirection direction = ConnectorDirection.Input;
        private ConnectorType type = ConnectorType.Entity;
        private ConnectorOrientation orientation = ConnectorOrientation.LeftToRight;

        public override List<Marker> CreateMarkers(Diagram diagram)
        {
            List<Marker> markers = new List<Marker>();
            return markers;
        }

        public ConnectorOrientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public Port(Figure main, Figure owner, ConnectorDirection dir, ConnectorType typ, ConnectorOrientation orient)
        {
            mainFigure = main;
            ownerFigure = owner;
            direction = dir;
            type = typ;
            textChangeEnable = false;
            Orientation = orient;
            NewTransparentRect();
        }

        public Port(Figure main, Figure owner, ConnectorDirection dir, ConnectorType typ)
        {
            mainFigure = main;
            ownerFigure = owner;
            direction = dir;
            type = typ;
            Orientation = ConnectorOrientation.LeftToRight;
            NewTransparentRect();
        }

        public Port()
        {
            NewTransparentRect();
        }

        public void NewTransparentRect()
        {
            FigurePen = Pens.Black;
            FigureBrush = Brushes.Black;
            //íåâèäèìûé ïðÿìîóãîëüíèê ïîðòà äëÿ "ëîâëè" ìàðêåðà
            Path.AddRectangle(new RectangleF(-defaultSize * 2, -defaultSize * 2, defaultSize * 4, defaultSize * 4));
            //óñòàíàâëèâàåì ñâîéñòâî: íåëüçÿ èçìåíÿòü ãðàíèöû
            resizeDirections = ResizeDirection.None;
        }

        public void Connect()
        {
            isConnected = true;
        }

        public void Disconnect()
        {
            isConnected = false;
        }

        public override void Draw(Graphics gr)
        {
            if (isConnected && direction == ConnectorDirection.Input)
            {
                gr.TranslateTransform(Location.X, Location.Y);
                List<Point> points = new List<Point>();
                switch (orientation)
                {
                    //ðèñóåì íàïðàâëåíèå ñòðåëîê â çàâèñèìîñòè îò òèïà ïîðòà
                    case ConnectorOrientation.LeftToRight:
                        points.Add(new Point(0, -defaultSize));
                        points.Add(new Point(0, defaultSize));
                        points.Add(new Point(defaultSize * 2, 0));
                        break;
                    case ConnectorOrientation.RightToLeft:
                        points.Add(new Point(defaultSize * 2, -defaultSize));
                        points.Add(new Point(defaultSize * 2, defaultSize));
                        points.Add(new Point(0, 0));
                        break;
                    case ConnectorOrientation.TopToBottom:
                        points.Add(new Point(-defaultSize, 0));
                        points.Add(new Point(defaultSize, 0));
                        points.Add(new Point(0, defaultSize * 2));
                        break;
                    case ConnectorOrientation.BottomToTop:
                        points.Add(new Point(-defaultSize, 0));
                        points.Add(new Point(defaultSize, 0));
                        points.Add(new Point(0, -2 * defaultSize));
                        break;
                }
                gr.FillPolygon(FigureBrush, points.ToArray());
                gr.DrawPolygon(FigurePen, points.ToArray());
                gr.ResetTransform();
            }
            else
            {
                gr.TranslateTransform(Location.X, Location.Y);
                gr.FillPath(Brushes.Transparent, Path);
                gr.ResetTransform();
            }
        }

        public CheckConnectionDelegate CheckConnection
        {
            get { return checkConnection; }
            set { checkConnection = value; }
        }

        public bool EnableMultipleConnection
        {
            get { return enableMultipleConnection; }
            set { enableMultipleConnection = value; }
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set { isConnected = value; }
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

        public ConnectorDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public ConnectorType Type
        {
            get { return type; }
            set { type = value; }
        }

        public Figure MainFigure
        {
            get { return mainFigure; }
            set { mainFigure = value; }
        }

        public bool IsConnectionOK()
        {
            if(CheckConnection != null)
                return CheckConnection();
            return true;
        }
    }
}
