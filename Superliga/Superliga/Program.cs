using SuperligaChallange.Model;
using SuperligaChallange.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperligaChallange
{
    class Program
    {
        static List<Socio> listaSocios = new List<Socio>();

        static Dictionary<string, EquipoEstadistica> listaEquipoEstadisticas = new Dictionary<string, EquipoEstadistica>();

        static List<Socio> listaSociosCasadosConEstudiosUniv = new List<Socio>();

        static Dictionary<string, int> nombresComunesHinchasRiver = new Dictionary<string, int>();

        static int cantidadSociosRacing = 0;

        static int edadSociosRacing = 0;


        static void Main(string[] args)
        {

            CargarSociosDeArchivoCsv("socios.csv");

            foreach (Socio unSocio in listaSocios)
            {
                if (unSocio.Equipo == "Racing")
                {
                    cantidadSociosRacing++;
                    edadSociosRacing += unSocio.Edad;
                }

                if (unSocio.EstadoCivil == "Casado" && unSocio.Estudios == "Universitario" &&
                    listaSociosCasadosConEstudiosUniv.Count < 100)
                {
                    listaSociosCasadosConEstudiosUniv.Add(unSocio);
                }

                if (unSocio.Equipo == "River")
                {
                    if (nombresComunesHinchasRiver.ContainsKey(unSocio.Nombre))
                    {
                        nombresComunesHinchasRiver[unSocio.Nombre]++;
                    }
                    else
                    {
                        nombresComunesHinchasRiver[unSocio.Nombre] = 1;
                    }
                }

                EquipoEstadistica equipoEstadistica;
                if (!listaEquipoEstadisticas.ContainsKey(unSocio.Equipo))
                {
                    equipoEstadistica = new EquipoEstadistica
                    {
                        Nombre = unSocio.Equipo
                    };

                    listaEquipoEstadisticas.Add(unSocio.Equipo, equipoEstadistica);
                }
                else
                {
                    equipoEstadistica = listaEquipoEstadisticas[unSocio.Equipo];
                }

                equipoEstadistica.CantidadSocios++;
                equipoEstadistica.SumaEdad += unSocio.Edad;

                if (equipoEstadistica.MayorEdad < unSocio.Edad)
                {
                    equipoEstadistica.MayorEdad = unSocio.Edad;
                }
                if (equipoEstadistica.MenorEdad > unSocio.Edad)
                {
                    equipoEstadistica.MenorEdad = unSocio.Edad;
                }

            }

            MostrarPorConsola();
        }

        static void MostrarPorConsola()
        {
            Console.WriteLine("---------- En total hay " + listaSocios.Count.ToString() + " personas registradas." + System.Environment.NewLine);

            Console.WriteLine("---------- El promedio de edad de los socios de Racing es de: " + (edadSociosRacing / cantidadSociosRacing).ToString() + " años" + System.Environment.NewLine);

            Console.WriteLine("---------- Las 100 primeras personas casadas, con estudios Universitarios, ordenadas de menor a mayor según su edad: " + System.Environment.NewLine);

            var result100PrimerasPersonas = from Socio unSocio
                                           in listaSociosCasadosConEstudiosUniv
                                            orderby unSocio.Edad
                                            select unSocio;

            foreach (Socio unSocio in result100PrimerasPersonas)
            {
                Console.WriteLine(unSocio.Nombre + ", " + unSocio.Edad.ToString() + " años, hincha de " + unSocio.Equipo + System.Environment.NewLine);
            }

            Console.WriteLine("---------- 5 nombres más comunes entre los hinchas de River: ----------" + System.Environment.NewLine);

            var resultNombresComunesHinchasRiver = (from t
                                               in nombresComunesHinchasRiver
                                                    orderby t.Value descending
                                                    select t).Take(5);

            foreach (KeyValuePair<string, int> i in resultNombresComunesHinchasRiver)
            {
                Console.WriteLine(i.Key + " se repite: " + i.Value.ToString() + " veces." + System.Environment.NewLine);
            }

            Console.WriteLine("---------- Listado ordenado de mayor a menor según la cantidad de socios, que enumere, "
                              + System.Environment.NewLine +
                              "junto con cada equipo, promedio de edad de sus socios, la menor edad registrada"
                              + System.Environment.NewLine +
                              "y la mayor edad registrada. ----------" + System.Environment.NewLine);

            foreach (KeyValuePair<string, EquipoEstadistica> i in listaEquipoEstadisticas)
            {
                Console.WriteLine(i.Value.Nombre + ": promedio de edad " + i.Value.PromedioDeEdad().ToString() + " años, menor edad " + i.Value.MenorEdad.ToString() + " años, mayor edad " + i.Value.MayorEdad.ToString() + " años." + System.Environment.NewLine);
            }

            Console.ReadLine();
        }

        static void CargarSociosDeArchivoCsv(string archivo)
        {
            var filas = System.IO.File.ReadAllLines(archivo);

            char[] separador = { ';' };

            foreach (string i in filas)
            {
                var columnas = i.Split(separador);

                Socio unSocio = new Socio
                {
                    Nombre = columnas[0],
                    Edad = Convert.ToInt32(columnas[1]),
                    Equipo = columnas[2],
                    EstadoCivil = columnas[3],
                    Estudios = columnas[4]
                };

                listaSocios.Add(unSocio);
            }
        }
    }
}