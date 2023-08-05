
//Classes
using ChargingScheduleGenerator;

JsonModel jsonModel = new JsonModel();
GenericClass genericClass = new GenericClass();

//Generates JSON file into readable content
jsonModel = genericClass.JsonToData();
//Gets the hours needed for the car to charge
double dHours = genericClass.HoursToCharge(jsonModel);
//this gets the plug in date and time to help determine when the car is plugged in and at what time it should start charging 
DateTime dtPlugInTime = genericClass.ConvertToDate(jsonModel.startingTime);
//Check if it needs to be charging now 
bool bCharging = genericClass.isCharging();
//Calculates the actual start date
DateTime dtStartDate = genericClass.CalculateStartDate(dtPlugInTime, jsonModel,bCharging);
//calculates the end date
DateTime dtEndDate = genericClass.CalculateEndDate(dtStartDate, dHours);
//converts the dates to their original formats 
string JsonStartDate = genericClass.DateToJson(dtStartDate);
string JsonEndDate = genericClass.DateToJson(dtEndDate);



Console.WriteLine(JsonStartDate);
Console.WriteLine(JsonEndDate);
Console.WriteLine(bCharging);
Console.ReadKey();




