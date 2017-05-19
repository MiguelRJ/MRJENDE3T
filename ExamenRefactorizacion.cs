
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MiguelRodriguez
{
    public class ArrayNuloException : Exception { }

    public class Examen
    {
        public static string Contar(int[] objetivo)
        {
            int numero, suma = 0, impares = 0, mayorPar = Int32.MinValue;//1
            decimal promedio = 0.0m;//1
            if (objetivo == null)//2
            {
                throw new ArrayNuloException();//E
            }
            for (int i = 0/*3*/; i < objetivo.Length/*4*/; i++/*10*/)
            {
                numero = objetivo[i];//5
                if (numero%2 != 0)//5
                {
                    impares++;//7
                    suma += numero;//7
                    promedio = suma / impares;//7
                }
                else
                {
                    if (numero>/*antes era '<' esto daba error*/mayorPar)//8
                    {
                        mayorPar = numero;//9
                    }
                }
            }//fin de for
            return "Mayor par: "+mayorPar+", Impar: "+impares+", Promedio de impares: "+promedio;//11
        }//12 //fin de Contar(int[] objetivo)

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MiguelRodriguez
{
    public class ArrayNuloException : Exception { }

    public class Examen
    {
        public static string Contar(int[] objetivo)
        {
            int numero, suma = 0, impares = 0, mayorPar = Int32.MinValue;//1
            decimal promedio = 0.0m;//1
            if (objetivo == null)//2
            {
                throw new ArrayNuloException();//E
            }
            for (int i = 0/*3*/; i < objetivo.Length/*4*/; i++/*10*/)
            {
                numero = objetivo[i];//5
                if (numero%2 != 0)//5
                {
                    impares++;//7
                    suma += numero;//7
                    promedio = suma / impares;//7
                }
                else
                {
                    if (numero>/*antes era '<' esto daba error*/mayorPar)//8
                    {
                        mayorPar = numero;//9
                    }
                }
            }//fin de for
            return "Mayor par: "+mayorPar+", Impar: "+impares+", Promedio de impares: "+promedio;//11
        }//12 //fin de Contar(int[] objetivo)

    }
}

