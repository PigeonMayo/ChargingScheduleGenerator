using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace ChargingScheduleGenerator
{
    internal class GenericClass
    {
        /// <summary>
        /// converts all the JSON data to a readable class file
        /// </summary>
        /// <returns>JsonModel which is a class i have created</returns>
        public JsonModel JsonToData()
        {
            JsonModel jsonModel = new JsonModel();
            string src = Path.Combine(Environment.CurrentDirectory, @"..\..\..\JsonData\inputJsonFile.json");


            using (StreamReader r = new StreamReader(src))
            {
                string json = r.ReadToEnd();
                jsonModel = JsonConvert.DeserializeObject<JsonModel>(json);
            }

            return jsonModel;
        }

        /// <summary>
        /// Calculates the total hours needed to charge the vehicle to the desired state
        /// </summary>
        /// <param name="item"></param>
        /// <returns>double</returns>
        public double HoursToCharge(JsonModel item)
        {
            /// Calculating the battery percentage by taking the current battery level dividing it by the battery Capacity and multiplying it by 100
            /// to see if it falls in the direct charging percentage value
            decimal batteryPerc = (item.carData.currentBatteryLevel / item.carData.batteryCapacity) * 100;
            /// Calculating the needed kW (kilowatt) needed to match desired state of charge
            decimal kwNeeded = (item.carData.batteryCapacity * item.userSettings.desiredStateOfCharge / 100) - item.carData.currentBatteryLevel;
            /// Calculating the time needed to charge to the desired state of charge
            double TimeToCharge = Convert.ToDouble(kwNeeded / item.carData.chargePower);

            return TimeToCharge;
        }

      

        /// <summary>
        /// converts the plug in time to a usable DateTime format
        /// </summary>
        /// <param name="startingTime"></param>
        /// <returns></returns>
        public DateTime ConvertToDate(string startingTime)
        {
            //splits date
            string date = startingTime.Substring(0, startingTime.IndexOf('T'));

            //splits time
            int pFrom = startingTime.IndexOf('T') + "T".Length;
            int pTo = startingTime.LastIndexOf('Z');
            String time = startingTime.Substring(pFrom, pTo - pFrom);

            //joined to compare date time values
            DateTime dateTime = DateTime.Parse(date + " " + time);

            return dateTime;
        }


        /// <summary>
        /// checks if the car should be currently charging  
        /// </summary>
        /// <returns>bool</returns>
        public bool BatteryLow(JsonModel item)
        {

            bool result = false;

            /// Calculating the battery percentage by taking the current battery level dividing it by the battery Capacity and multiplying it by 100
            /// to see if it falls in the direct charging percentage value
            decimal batteryPerc = (item.carData.currentBatteryLevel / item.carData.batteryCapacity) * 100;

            if (batteryPerc < 20)
            {
                result = true;

            }

            return result;
        }


        /// <summary>
        /// Determines if the battery needs to charge for this period of time
        /// </summary>
        /// <param name="jsonModel"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool IsCharging(DateTime dtStartDate, DateTime dtEndDate)
        {
            DateTime currentTime = DateTime.Now;

            bool bResult = (currentTime >= dtStartDate && currentTime <= dtEndDate ? true :false);

            return bResult;
        }

        /// <summary>
        /// calculates when the car needs to start charging adds a day if it only charges tomorrow or it keeps the date now if it needs to charge now 
        /// also adds time 
        /// </summary>
        /// <param name="dtPlugInTime"> the date and time the vehicle was plugged in </param>
        /// <param name="jsonModel"> the model to access the timesa and tariffs</param>
        /// <param name="bCharging"> the indicator if the car is charging now </param>
        /// <returns>DateTime</returns>
        public DateTime CalculateStartDate(DateTime dtPlugInTime, JsonModel jsonModel, bool bCharging)
        {
            DateTime StartDate = dtPlugInTime;
            decimal lowesttariff = jsonModel.userSettings.tariffs.Min(tariffcompare => tariffcompare.energyPrice); //gets the minimum price
            string timeGiven = dtPlugInTime.ToString("HH:mm");
            string timeTariff = "";

            //if its less than 20% it needs to charge now 
            if (bCharging == true)
            {
                return StartDate;
            }

            foreach (var tariffdata in jsonModel.userSettings.tariffs)
            {
                timeTariff = (tariffdata.energyPrice == lowesttariff ? tariffdata.startTime : timeTariff);
            }

            DateTime dateTime1 = DateTime.ParseExact(timeGiven, "HH:mm", null);
            DateTime dateTime2 = DateTime.ParseExact(timeTariff, "HH:mm", null);

            //if time is less then its the next day
            //if its more then its the current day
            StartDate = (DateTime.Compare(dateTime1, dateTime2) < 0 ? StartDate.AddDays(1) : StartDate);

            StartDate = Convert.ToDateTime(StartDate.Date.ToString("dd/MM/yyyy") + " " + timeTariff);

            return StartDate;
        }

        /// <summary>
        /// adds the amount of time it will take for the car to charge to the start date thus producing the time it needs to cut off charge
        /// </summary>
        /// <param name="dtStartDate">the start date and time of charge</param>
        /// <param name="dHours">the amount of hours it will take to charge</param>
        /// <returns>DateTime</returns>
        public DateTime CalculateEndDate(DateTime dtStartDate, double dHours)
        {
            DateTime EndDate = dtStartDate;

            EndDate = EndDate.Add(TimeSpan.FromHours(Math.Round(dHours, 2)));


            return EndDate;

        }

        /// <summary>
        /// Converts the date and time to a JSON format for output 
        /// </summary>
        /// <param name="DateToConvert">the datetime that needs to be changed</param>
        /// <returns>DateTime</returns>
        public string DateToJson(DateTime DateToConvert)
        {
            string Date = "";

            Date = DateToConvert.ToString().Replace("/", "-").Replace(" ", "T") + ":00Z";

            return Date;
        }


        public void WriteToFile(DateTime dtStartDate, DateTime dtEndDate, bool bCharging)
        {
            JsonDataWrite data = new JsonDataWrite
            {
                startTime = DateToJson(dtStartDate),
                endTime = DateToJson(dtEndDate),
                isCharging = bCharging,

            };

            // Convert the instance to JSON
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);

            // Specify the path to the JSON file
            string filePath = Path.Combine(Environment.CurrentDirectory, @"..\..\..\JsonData\OutputJsonFile.json");

            // Write the JSON to the file
            File.WriteAllText(filePath, json);

        }
    }
}
