using System;
using System.Drawing.Drawing2D;
using System.Drawing;

using sapr_sim.Figures.Basic.Enums;
using sapr_sim.Figures.Custom.Enum;

namespace sapr_sim.Figures.Basic
{
    public interface IConnectable
    {
        void Connect();
        void Disconnect();
        bool IsConnected { get; set; }
        bool EnableMultipleConnection { get; set; }
        ConnectorOrientation Orientation { get; set; }
        PointF Location { get; set; }
        float X { set; get; }
        float Y { set; get; }
        Figure MainFigure { get; set; }
        Figure OwnerFigure { get; set; }
        ConnectorType Type { get; set; }
        ConnectorDirection Direction { get; set; }
        //CheckConnectionDelegate CheckConnection { get; set; }
        bool IsConnectionOK();
    }
}
