//tomsaji
//february 4, 2026
using System;
namespace HelloWorld
{
    class Tax
    {
        static void Main(string[] args){
         
         double tax = 0;
         bool validInput = false;
         double userInput = 0;
         const double taxLowest = 1.00;
         const double taxLowest2 = 4461.99;
         
         const double taxLow =  4462.0; 
         const double taxLow2 = 17893.99;
         
         const double taxMid = 17894.0;
         const double taxMid2 = 29499.99;
         
         const double taxHigh = 29500.0;
         const double taxHigh2 = 45787.99;
         
         const double taxHighest = 45788.0;





           while(!validInput){

            try{
                Console.WriteLine("How much money did you make this year?");
                userInput = Convert.ToDouble(Console.ReadLine());
                if (userInput < 1 ) {
                 Console.WriteLine("Enter a number in range ");
                  continue;
                
                  }
                validInput = true;   
               }
            catch{
                Console.WriteLine("Numbers only!");
                
            }
           }
           if (taxLowest <= userInput && userInput <= taxLowest2)
            {
                tax = 0;
            }
           
           else if (taxLow <= userInput && userInput <= taxLow2)
            {              
                 tax = (userInput - taxLow) * .30;
            }
           
            else if (taxMid <= userInput && userInput <= taxMid2)
            {
               
                double flatTax = 4119.00;
                 tax = (userInput - taxMid) * .35 + flatTax;
            }
           
            
            else if (taxHigh <= userInput && userInput <= taxHigh2)
            {
                 double flatTax = 8656.00;
                 tax = (userInput - taxHigh) * .46 + flatTax;
            }
           
            else if (taxHighest <= userInput)
            {
              
                double flatTax = 11179.00;
                 tax = (userInput - taxHighest) * .60 + flatTax;
            }
            else {
                Console.WriteLine("no");
            }

            Console.WriteLine($"You will be paying this much tax: {tax:C}");


        }

    }

}

