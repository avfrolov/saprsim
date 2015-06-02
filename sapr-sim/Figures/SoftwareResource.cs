using sapr_sim.Parameters;
using sapr_sim.Parameters.Validators;
using sapr_sim.Utils;
using sapr_sim.WPFCustomElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace sapr_sim.Figures
{
    [Serializable]
    public class SoftwareResource : Resource, ISerializable
    {

        private UIParam<Double> price = new UIParam<Double>(1, new BetweenDoubleParamValidator(0.0, 100000.0), "Цена",
            "Цена данного программного обеспечения. Может принимать вещественное значение на отрезке [0; 100000]");
        private UIParam<Int32> licenseCount = new UIParam<Int32>(1, new BetweenIntegerParamValidator(1, 1000), "Количество копий",
            "Количество лицензий данного оборудования. Может принимать целочисленное значение на отрезке [1; 1000]");
        private UIParam<Double> time = new UIParam<Double>(1, new PositiveDoubleParamValidator(), "Продолжительность",
            "Время действия лицензии. Может принимать положительное целочисленное значение");
        private UIParam<TimeMeasure> timeMeasure = new UIParam<TimeMeasure>(TimeMeasure.SECOND, new DefaultParamValidator(), "Единицы измерения",
            "Единица измерения времени", new ParameterComboBox(TimeMeasure.list()));
        private UIParam<Double> failure = new UIParam<Double>(0.1, new BetweenDoubleParamValidator(0.0, 1.0), "Вероятность ошибки",
            "Вероятность ошибки ресурса. Может принимать вещественное значение на отрезке [0; 1]");

        public SoftwareResource(Canvas canvas) : base(canvas)
        {
            init();
            name.Value = ResourceType.SOFTWARE.Name;
            type.Value = ResourceType.SOFTWARE;
        }

        public SoftwareResource(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            price = info.GetValue("price", typeof(UIParam<Double>)) as UIParam<Double>;
            licenseCount = info.GetValue("licenseCount", typeof(UIParam<Int32>)) as UIParam<Int32>;
            time = info.GetValue("time", typeof(UIParam<Double>)) as UIParam<Double>;

            timeMeasure = info.GetValue("timeMeasure", typeof(UIParam<TimeMeasure>)) as UIParam<TimeMeasure>;
            timeMeasure.ContentControl = new ParameterComboBox(TimeMeasure.list()) { SelectedIndex = timeMeasure.Value.Order };

            failure = info.GetValue("failure", typeof(UIParam<Double>)) as UIParam<Double>;

            init();
        }

        public override void createAndDraw(double x, double y)
        {
            base.createAndDraw(x, y);

            label = new Label(this, canvas, x + 40, y + 22, name.Value);
            canvas.Children.Add(label);
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = base.getParams();
            param.Add(price);
            param.Add(licenseCount);
            param.Add(time);
            param.Add(timeMeasure);
            param.Add(failure);
            return param;
        }

        public double Price
        {
            get { return price.Value; }
            set { price.Value = value; }
        }

        public int LicenseCount
        {
            get { return licenseCount.Value; }
            set { licenseCount.Value = value; }
        }

        public double Failure
        {
            get { return failure.Value; }
            set { failure.Value = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("price", price);
            info.AddValue("licenseCount", licenseCount);
            info.AddValue("time", time);
            info.AddValue("timeMeasure", timeMeasure);
            info.AddValue("failure", failure);
        }

        protected override void init()
        {
            base.init();
            Fill = Brushes.AliceBlue;
        }
    }
}
