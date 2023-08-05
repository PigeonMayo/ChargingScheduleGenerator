
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
//Check if the battery is below 20% if it is then return true if it isnt then return false (Hugo)
bool bCharging = genericClass.isCharging();
//Calculates the actual start date
DateTime dtStartDate = genericClass.CalculateStartDate(dtPlugInTime, jsonModel,bCharging);
//calculates the end date
DateTime dtEndDate = genericClass.CalculateEndDate(dtStartDate, dHours);
///add a method here to check if the time right now is between the the start time and end time if it is return a true if it isnt return a false 
///remember to change the value of bCharging with this method (Hugo)

//converts the dates to their original formats 
string JsonStartDate = genericClass.DateToJson(dtStartDate);
string JsonEndDate = genericClass.DateToJson(dtEndDate);

///add a method here that prints out the data the way they want it. use the parameters below (hugo)


Console.WriteLine(JsonStartDate);
Console.WriteLine(JsonEndDate);
Console.WriteLine(bCharging);
Console.ReadKey();




