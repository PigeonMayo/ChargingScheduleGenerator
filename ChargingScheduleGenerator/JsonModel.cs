using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingScheduleGenerator
{

    public class JsonModel
    {
        public string startingTime { get; set; }
        public UserSettings userSettings { get; set; }
        public CarData carData { get; set; }
    }

    public class UserSettings
    {
        public int desiredStateOfCharge { get; set; }
        public string leavingTime { get; set; }
        public int directChargingPercentage { get; set; }
        public List<Tariffs> tariffs { get; set; }
    }

    public class Tariffs
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public decimal energyPrice { get; set; }
    }

    public class CarData
    {
        public decimal chargePower { get; set; }
        public decimal batteryCapacity { get; set; }
        public decimal currentBatteryLevel { get; set; }
    }

    public class JsonDataWrite
    {
        public  string startTime { get; set; }
        public  string endTime { get; set; }
        public  bool isCharging { get; set; }
    }


}
