using System;

namespace HelloWorld
{
  class Program
  {
     static void Main(string[] args)
    {
        int age = 0;
        int add = 0;
        int income = 0;
        int jobStability = 0;
        int agePoints = 0;
        int addPoints = 0;
        int incomePoints = 0;
        int jobStabilityPoints = 0;

         Console.WriteLine("Enter your age:");
         age = Convert.ToInt32(Console.ReadLine());
         if (age <= 20 ){
            agePoints = -10;
          }
          else if ( age >= 21 && age <= 30 ){
            agePoints = 0;
          }
          else if ( age >= 31 && age <= 50 ){
            agePoints = 20;
          }
          else {
            agePoints = 25;
          }

         Console.WriteLine("Enter your years at current address :");
         add = Convert.ToInt32(Console.ReadLine());
         if (add < 1){
            addPoints = -5;
          }
          else if ( add >= 1 && add <= 3){
            addPoints = 5;
          }
          else if ( add >= 4 && add <= 8){
            addPoints = 12;
          }
          else {
            addPoints = 20;
          }

         Console.WriteLine("Enter your Annual Income:");
         income = Convert.ToInt32(Console.ReadLine());
         if (income <= 15000){
            incomePoints = 0;
          }
          else if ( income >= 15001 && income <= 25000){
            incomePoints = 12;
          }
          else if ( income >= 25001 && income <= 40000){
            incomePoints = 24;
          }
          else {
            incomePoints = 30;
          }

         Console.WriteLine("Enter your Years at same Job :");
         jobStability = Convert.ToInt32(Console.ReadLine());
         if (jobStability < 2){
            jobStabilityPoints = -4;
          }
          else if ( jobStability >= 2 && jobStability <= 4 ){
            jobStabilityPoints = 8;
          }
          
          else {
            jobStabilityPoints = 15;
          }
          int creditCard = jobStabilityPoints + addPoints + incomePoints + agePoints;
          string card;
           if ( creditCard >= 21 && creditCard <=35){
            card = "$500";
            
           }
           else if (creditCard >= 36 && creditCard <=60) {
            card = "$2000";

           }
           else if (creditCard >= 61){
            card = "$5000";

           }
           else {
            card = "No Credit Card was Issued";

           }

        Console.WriteLine("------------------------------------------");
        Console.WriteLine("|        CREDIT CARD EVALUATION           |");
        Console.WriteLine("------------------------------------------");
        Console.WriteLine($"| Age:                        {age,-10} |");
        Console.WriteLine($"| Years at Address:           {add,-10} |");
        Console.WriteLine($"| Annual Income:              {income,-10} |");
        Console.WriteLine($"| Years at Same Job:          {jobStability,-10} |");
        Console.WriteLine("------------------------------------------");
        Console.WriteLine($"| Credit Card Decision: {card,-17} |");
        Console.WriteLine("------------------------------------------");


      
      



   


    }  
  }
}
