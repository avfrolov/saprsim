using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using System.Linq;
using System.ComponentModel;

using sapr_sim.Figures.Basic.Lines;
using sapr_sim.Figures.Basic;


namespace sapr_sim.Figures.Custom
{

    public class Diagram : ICloneable
    {

        private string name;
        private readonly List<Figure> figures = new List<Figure>();

        public List<Figure> Figures 
        {
            get { return figures; }
        }

        //сохранение диаграммы в файл
        public void Save(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
                new BinaryFormatter().Serialize(fs, this);
        }

        public void Save(FileStream fs)
        {
            new BinaryFormatter().Serialize(fs, this);
        }

        //чтение диаграммы из файла
        public static Diagram Load(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
                return (Diagram)new BinaryFormatter().Deserialize(fs);
        }

        public static Diagram Load(FileStream fs)
        {
            return (Diagram)new BinaryFormatter().Deserialize(fs);
        }

        public object Clone()
        {
            //создаем новую диаграмму, куда копируем фигуры из текущей диаграммы
            Diagram d = new Diagram();
            d.name = name;
            int i, j, Q;
            ComplexFigure tmp;
            Line line;
            //копирование фигур в диаграмму
            for (i = 0; i < this.figures.Count; i++)
                d.figures.Add(this.figures[i].Clone() as Figure);
            //установление связей между скопированными линиями и скопированными фигурами
            for (i = 0; i < d.figures.Count; i++)
            {
                if (d.figures[i] is Line)
                {
                    //нашли линию
                    line = (d.figures[i] as Line);
                    Q = 0;//число переподключенных концов
                    for (j = 0; j < d.figures.Count; j++)
                    {
                        //нашли некую сложную фигуру
                        if (d.figures[j] is ComplexFigure)
                        {
                            tmp = (d.figures[j] as ComplexFigure);
                            if (line.From == null)
                                Q++;
                            else if (line.From.OwnerFigure == null)
                                Q++;//один конец фигуры линии уже подключен к "ничему"
                            else if (line.From.OwnerFigure.Name == tmp.Name)
                            {
                                //определили, что линия к ней подключена точкой From
                                foreach (SolidFigure connector in tmp.SolidFigures)
                                {
                                    if ((line.From as Figure).Name == connector.Name)
                                    {
                                        line.From = connector as IConnectable;
                                        Q++;
                                        break;
                                    }
                                }
                            }
                            if (line.To == null)
                                Q++;
                            else if (line.To.OwnerFigure == null)
                                Q++;//один конец фигуры линии уже подключен к "ничему"
                            else if (line.To.OwnerFigure.Name == tmp.Name)
                            {
                                //определили, что линия к ней подключена точкой From
                                foreach (SolidFigure connector in tmp.SolidFigures)
                                {
                                    if ((line.To as Figure).Name == connector.Name)
                                    {
                                        line.To = connector as IConnectable;
                                        Q++;
                                        break;
                                    }
                                }
                            }
                            if (Q >= 2) //значит, линию уже полностью переподключили
                                break;
                        }
                    }
                }
            }
            return d as Object;
        }
    }

    public delegate bool CheckConnectionDelegate();
    public delegate void ConnectBlocksDelegate(string NameFrom, string NameTo, int param);
    public delegate void DisconnectBlocksDelegate(string NameFrom, string NameTo);
}
