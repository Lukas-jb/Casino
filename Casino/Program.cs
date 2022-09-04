using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*Un  casino  requiere  un  programa  para  una  maquina  dispensadora  de  fichas para jugar, los clientes poseen
             * una cuenta  con  un  saldo  (similar  a  una  cuenta  bancaria).  El  usuario  se  debe  registrar  al  inicio
             * con  el  nombre  y  una clave para poder ingresar y el programa calcular la cantidad mínimas de fichas y de que
             * denominación se deben entregar a un usuario. A la maquina se le puede ingresar cualquier valor inferior a 5000
             * pesos para cambiar y ella posee  fichas  de  1000,  500,  200,  100  y  50  pesos.  Si  el  valor  restante  está
             * entre  49  y  25  pesos  se  aproxima  a  50 pesos y si es de 1 a 24 se trunca a 0.*/
            
            string User;
            string Password;
            string Message = "";
            int Balance;
            int Coin = 0;
            int CasinoChipType = 0;
            int Module = 0;
            bool varValidatePassword = false;
            bool varValidateUser = false;
            bool varValidateDrop = false;
            const int MaxDrop = 5000;

            Console.WriteLine("BIENVENIDO AL PROGRAMA DE MÁQUINA DISPENSADORA\n");

            Console.WriteLine("Modulo de registro de usuario");
            Console.WriteLine("Ingrese un usuario ha almacenar");
            User = Console.ReadLine();
            Console.WriteLine("Ingrese una contrasela");
            Password = Console.ReadLine();
            do
            {
                Console.WriteLine("Ingrese Saldo a la cuenta");
                Balance = Convert.ToInt32(Console.ReadLine());
                if (Balance < 0) Console.WriteLine("Ingrese un valor mayor a 0\n");
            } while (Balance < 0);

            Console.WriteLine("*********** Infomarción almacenada con exito ***********");
            Console.WriteLine("Bienvenido al sistema de cambio");
            if (User != null || Password != null)
            {
                string inUser;
                string inPassword;
                int count = 0;
                int Drop;
                bool NewDrop = false;
                do
                {
                    do
                    {
                        Console.WriteLine("Ingrese el usuario ");
                        inUser = Console.ReadLine();
                        varValidateUser = ValidateUder(inUser, User);

                        if (varValidateUser == false)
                        {
                            Console.WriteLine("Usuario ingresado no valido, intentelo de nuevo");
                            count++;
                        }
                        if (count == 3)
                        {
                            Console.WriteLine("Supero el limite de intentos");
                            CloseProgram();
                        }
                    } while (varValidateUser == false);

                    do
                    {
                        Console.WriteLine("Ingrese la contrasela");
                        inPassword = Console.ReadLine();
                        varValidatePassword = ValidatePassword(inPassword, Password);
                        if (varValidatePassword == false)
                        {
                            Console.WriteLine("Usuario ingresado no valido, intentelo de nuevo");
                            count++;
                        }
                        if (count == 3)
                        {
                            Console.WriteLine("Supero el limite de intentos");
                            CloseProgram();
                        }
                    } while (varValidatePassword == false);

                    do
                    {
                        Console.WriteLine("Ingrese el monto a retirar ");
                        Drop = Convert.ToInt32(Console.ReadLine());
                        varValidateDrop = ValidateDrop(Drop, Balance, MaxDrop);
                    } while (varValidateDrop == false);

                    CalculeCoin(ref Message, ref Coin, 1000, ref Drop, ref Balance);
                    CalculeCoin(ref Message, ref Coin, 500, ref Drop, ref Balance);
                    CalculeCoin(ref Message, ref Coin, 200, ref Drop, ref Balance);
                    CalculeCoin(ref Message, ref Coin, 100, ref Drop, ref Balance);
                    CalculeCoin(ref Message, ref Coin, 50, ref Drop, ref Balance);

                    Console.WriteLine("\nLas fichas de casino entregadas son: \n");
                    Console.WriteLine(Message + "\n");

                    bool validation = false;
                    do
                    {
                        string Desicion;
                        Console.WriteLine("¿Desea realizar otro retiro?\n");
                        Console.WriteLine("(Si - 'Y')  (No - 'N')\n");
                        Desicion = Console.ReadLine().ToUpper();

                        NewDrop = (Desicion == "Y") ? true : false;

                        validation = (Desicion == "N" || Desicion == "Y") ? true : false;

                    } while (validation == false);
                    Message = string.Empty;

                } while (NewDrop == true);

            }
            Console.WriteLine("***************** Retiro exitoso *****************\n (\\__/)\r\n(>’.’<)\r\n(\")_(\")");
            Console.ReadKey();
        }

        private static void CalculeCoin(ref string Message, ref int Coin, int CasinoChipType, ref int Drop, ref int Balance)
        {
            while ((Drop / CasinoChipType >= 1) || (CasinoChipType == 50 && Drop % CasinoChipType >= 1))
            {
                while (Drop / CasinoChipType >= 1)
                {
                    Coin = Drop / CasinoChipType;
                    Drop = Drop % CasinoChipType;
                }

                while (CasinoChipType == 50 && Drop % CasinoChipType >= 1)
                {
                    Coin = Coin + Drop / CasinoChipType;
                    if (Drop <= 49 && Drop >= 25)
                    {
                        Coin = Coin + 1;
                        Console.WriteLine("Se aplicareajuste segun los criterios del casino: ");
                    }
                    Drop = 0;       
                }
                Message = ConcatMessage(Message, Coin, CasinoChipType);
                Balance = Balance - (Coin * CasinoChipType);
                Coin = 0;
            }//balance = balance - (coin * CasinoChipType)
        }

        public static bool ValidateUder(string Intuser, string User)
        {
            return Intuser == User ? true : false;
        }
        public static bool ValidatePassword(string inPassword, string Password)
        {
            return inPassword == Password ? true : false;
        }
        public static bool ValidateDrop(int Drop, int balance, int MaxDrop)
        {
            bool DropOk = true;
            if (Drop > balance)
            {
                Console.WriteLine("El monto a retirar no puede exceder el saldo, saldo actual : " + balance);
                DropOk = false;
            }
            if (Drop > MaxDrop)
            {
                Console.WriteLine(" Valor ingresado supera el maximo permitido, nomton maximo: " + MaxDrop);
                DropOk = false;
            }
            if (Drop < 0)
            {
                Console.WriteLine("Valor ingresado no valido, debe ser mayor a 0");
                DropOk = false;
            }
            return DropOk;
        }

        public static string ConcatMessage(string Message, int coin, int CasinoChipType)
        {
            string m1 = coin < 2 ? " moneda de " : " monedas de ";

            Message = Message + coin + m1 + CasinoChipType + ", ";
            return Message;
        }
        static void CloseProgram()
        {
            Console.WriteLine("Precione cualquier tecla para cerrar");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
