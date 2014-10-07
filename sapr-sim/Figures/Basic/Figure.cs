using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.ComponentModel;

using sapr_sim.Figures.Basic.Util;
using sapr_sim.Figures.Basic.Markers;

namespace sapr_sim.Figures.Basic
{
    public abstract class Figure : ICloneable
    {
        //идентификатор
        protected string name;
        protected static Random rnd = new Random(DateTime.Now.Second);

        public delegate bool CheckConnectionDelegate();
        public delegate void ConnectBlocksDelegate(string NameFrom, string NameTo, int param);
        public delegate void DisconnectBlocksDelegate(string NameFrom, string NameTo);

        public Figure()
        {
            name = this.GetHashCode().ToString(); 
        }

        //линии фигуры
        public SerializableGraphicsPath serializablePath = new SerializableGraphicsPath();
        public GraphicsPath Path
        {
            get { return serializablePath.path; }
            set { serializablePath.path = value; }
        }
        //карандаш отрисовки линий
        public Pen FigurePen
        {
            get { return serializablePath.pen; }
            set { serializablePath.pen = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //точка находится внутри фигуры?
        public abstract bool IsInsidePoint(Point p);

        //отрисовка фигуры
        public abstract void Draw(Graphics gr);

        //получение маркеров
        public abstract List<Marker> CreateMarkers(Diagram diagram);

        //смещение фигуры
        public abstract void Offset(float dx, float dy);

        //прямоугольник вокруг фигуры (в абсолютных координатах)
        public abstract RectangleF Bounds
        {
            get;
        }

        public abstract object Clone();
    }
}
