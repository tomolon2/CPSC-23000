using System;

namespace Loan 
{
    class LoanApp
    {
        static void Main(string[] args )
        {
            double minLoan = 5000;
            double maxLoan = 500000;
            int paymentYears = 0;
            int paymentRate = 12;
            double interestRate = 0;
            bool validInput = false;
             double loanPrinciple = 0;
            
           while (!validInput){
            Console.WriteLine("What is the principle amount for loan: 5,000 to 500,000");
             loanPrinciple = Convert.ToDouble(Console.ReadLine());
            if (loanPrinciple >= minLoan && loanPrinciple <= maxLoan){ 
                validInput = true;
            }
            else{
                Console.WriteLine("B");
            }
           } 

           Console.WriteLine("What is yearly interest rate?: ");
           interestRate = Convert.ToDouble(Console.ReadLine());
           double interestConvert = interestRate / 100;

           Console.WriteLine("How many years to payback loan?: ");
           paymentYears = Convert.ToInt32(Console.ReadLine());

           double monthlyPayment = PaymentCalculator(loanPrinciple,interestConvert,paymentYears,paymentRate);
           Console.WriteLine($"Monthly Payment: {monthlyPayment:C}");

           double balance = loanPrinciple;
           double totalInterest = 0;
           DateTime startDate = new DateTime(2026, 2, 4);

           Console.WriteLine("Loan Repayment Schedule:");
           Console.WriteLine("Payment#  Date        Payment    Interest   Principal    Balance");
           Console.WriteLine($"0         {startDate.ToShortDateString()}    $0.00      $0.00       $0.00       {balance:C}");

           for (int i = 1; i <= paymentYears * paymentRate; i++)
             {
                double interestPayment = balance * (interestConvert / paymentRate);
                double principalPayment = monthlyPayment - interestPayment;
                balance -= principalPayment;
                totalInterest += interestPayment;
                if (balance < 0) balance = 0;
                 DateTime paymentDate = startDate.AddMonths(i);
                 Console.WriteLine($"{i,-10} {paymentDate.ToShortDateString(),-10} {monthlyPayment,9:C} {interestPayment,9:C} {principalPayment,11:C} {balance,11:C}");
             }

                 Console.WriteLine($"\nTotal Payment: {monthlyPayment * paymentYears * paymentRate:C}");
                 Console.WriteLine($"Total Interest: {totalInterest:C}");


           
            
        }

        static double PaymentCalculator(double loan, double interest, int years, int paymentRate)
        {
            double n = years * paymentRate;
            double r = interest/paymentRate;

            return (loan * r)/(1- Math.Pow(1 +r , -n));

        }

        }
    }
