using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Transactions;

namespace SQL_Tracker
{
    
   

    internal class Program
    {
        static void Main(string[] args)
        {
            SqlConnection con = new SqlConnection("Server=IN-5YC79S3; database=TestDB; Integrated Security=true");
            con.Open();

            string res = "y";
            do
            {
                Console.WriteLine("1. Add Transaction");
                Console.WriteLine("2. View Expenses");
                Console.WriteLine("3. View Income");
                Console.WriteLine("4. Check Available Balance");
                Console.WriteLine("Enter your Choice");
                int choice = Convert.ToInt16(Console.ReadLine());
               
                switch (choice)
                {
                    case 1:
                        {
                            Console.WriteLine("Enter Title");
                            string title = Console.ReadLine();
                            Console.WriteLine("Enter Description");
                            string description = Console.ReadLine();
                            Console.WriteLine("Enter Amount");
                            int amount = Convert.ToInt16(Console.ReadLine());
                            DateTime date = new DateTime();
                            try
                            {
                                Console.Write("Enter Date(DD/MM/YYYY): ");
                                date = DateTime.Parse(Console.ReadLine());
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Enter in format dd/mm/yyyy");
                                return;
                            }

                            if (amount < 0)
                            {
                                int Expenses = 0;
                                Console.WriteLine("Expense Transactions");
                                Expenses += Math.Abs(amount);
                            }
                            else
                            {
                                int Income = 0;
                                Console.WriteLine("Income Transactions");
                                Income += amount;
                            }

                            SqlCommand cmd = new SqlCommand("insert into  Tracker values (@title, @description , @amount , @date )", con);
                            cmd.Parameters.AddWithValue("@title", title);
                            cmd.Parameters.AddWithValue("@description", description);
                            cmd.Parameters.AddWithValue("@amount", amount);
                            cmd.Parameters.AddWithValue("@date", date);
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Recorded saved successfully");
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Expenses:");
                            SqlCommand cmd2 = new SqlCommand("select * from Tracker where amount<0 ",con);
                            SqlDataReader dr = cmd2.ExecuteReader();
                            while (dr.Read())
                            {
                                Console.WriteLine($"{dr[0]}\t{dr[1]}\t{dr[2]}\t{dr[3]} ");

                            }
                            dr.Close();
                            break;

                        }
                    case 3:
                        {
                            
                            Console.WriteLine("Income:");
                            SqlCommand cmd3 = new SqlCommand("select * from Tracker where amount>0 ", con);
                            SqlDataReader dr = cmd3.ExecuteReader();
                            while (dr.Read())
                            {
                                Console.WriteLine($"{dr[0]}\t{dr[1]}\t{dr[2]}\t{dr[3]} ");
                                /*for (int i = 0; i < dr.FieldCount; i++)
                                {
                                    Console.WriteLine($"{dr[i]}\t");
                                }
                                Console.WriteLine();*/
                            }
                            dr.Close();
                            break;
                        }
                    case 4:
                        {
                            
                            SqlCommand cmd4 = new SqlCommand("select sum(amount) as AvailableBalance from Tracker", con);
                            int balance = (int)cmd4.ExecuteScalar();
                            Console.WriteLine($"Available Balance is {balance}");
                            break;
                            

                        }
                    default:
                        {
                            Console.WriteLine("Wrong Choice Entered");
                            break;
                        }

                }
                Console.WriteLine("Do you wish to continue? [y/n] ");
                res = Console.ReadLine();
            } while (res.ToLower() == "y");
            con.Close();

        }
    }
}