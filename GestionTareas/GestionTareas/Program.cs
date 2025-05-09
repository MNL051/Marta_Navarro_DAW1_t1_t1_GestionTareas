﻿using System;
class Program
{
     static List <Tarea> tareas = new List<Tarea>();
    static void Main(string[] args)
    {
       bool continuar = true;
       while (continuar){

        Console.WriteLine("\nMenú de tareas, por favor seleccione una opción: ");
        Console.WriteLine("1.Crear tarea");
        Console.WriteLine("2.Buscar tarea por tipo");  
        Console.WriteLine("3.Eliminar tarea");
        Console.WriteLine("4.Exportar tarea");
        Console.WriteLine("5.Importar tarea");

        string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    CrearTarea();
                    break;

                case "2":
                    BuscarTarea();
                    break;

                case "3":
                    ElminarTarea();
                    break;

                case "4":
                    ExportarTareas();
                    break;

                case "5":
                    ImportarTareas(tareas);
                    break;


                default:
                    Console.WriteLine("\nTérmino erróneo. Intenteló de nuevo");
                    break;
            }

        }
    }

    static void CrearTarea(){ 

        Console.WriteLine("Ingrese el nombre de la tarea: ");
        string? nombreTarea = Console.ReadLine();

        Console.WriteLine("Ingrese una breve descripcion: "); 
        string? descripcionTarea = Console.ReadLine(); 

        Console.WriteLine("¿De que tipo es la tarea? ¿Persona, Trabajo u Ocio?: ");
        string? tipo = Console.ReadLine();
        TipoTarea tipoTarea; //para inicializar el tipo de tarea es necesrario trasnformar el string a un enum 
        while (!Enum.TryParse(tipo, true, out tipoTarea))
        {
            Console.WriteLine("Tipo erróneo. Por favor, ingrese un tipo válido (Persona, Trabajo u Ocio): ");
            tipo = Console.ReadLine();
        }

        Console.WriteLine("¿La tarea es prioritaria? (Si/No): ");
          string? respuestaPrioridad = Console.ReadLine();
          bool prioridadBool = respuestaPrioridad != null && (respuestaPrioridad.ToLower() == "sí" || respuestaPrioridad.ToLower() == "si");
          //.ToLower() para que no importe si se escribe en mayuscula o minuscula, y == "si" para que no importe si la persona escribe "si" o "sí"
      
        tareas.Add(new Tarea{NombreTarea = nombreTarea, DescripcionTarea = descripcionTarea, Tipotarea = tipoTarea, PrioridadTarea = prioridadBool});
        Console.WriteLine("Tarea registrada");

        Console.WriteLine("\nPresione cualquier tecla para volver al menú: ");
        Console.ReadKey();

    }
 
     static void BuscarTarea(){  

        Console.WriteLine("¿Que tipo de tarea desea buscar (Persona, Trabajo u Ocio)?");

        string? tipo = Console.ReadLine();
        TipoTarea tipoTarea;
         while (!Enum.TryParse(tipo, true, out tipoTarea))
        {
            Console.WriteLine("Tipo erróneo. Por favor, ingrese un tipo válido (Persona, Trabajo u Ocio): ");
            tipo = Console.ReadLine();
        } 

        var tareasFiltradas = tareas.Where(t => t.Tipotarea == tipoTarea).ToList();
        //tareasFiltradas para guardar las tareas solicitadas
        /* where para filtrar, donde cada tarea "t" de la lista me de solo las que sean iguales a las del tipo que busco */

        if (tareasFiltradas.Any())
        /*"Any" me verifica si hay un elemento en la lista y me devuelve un true si hay o un false si esta vacia */
        {
            Console.WriteLine("\nTareas del tipo " + tipoTarea + " encontradas:");
            foreach (var tarea in tareasFiltradas)
            {
                string prioridadTexto = tarea.PrioridadTarea == true ? "Alta" : "Baja";  //para asignar el texto de la prioridad, True = Alta, False = Baja
                
                //Console.WriteLine("Nombre: " + tarea.NombreTarea +", Descripción: " + tarea.DescripcionTarea +", Prioridad: " + tarea.PrioridadTarea);
                Console.WriteLine("Id: " + tarea.IdTarea);
                Console.WriteLine("Nombre: " + tarea.NombreTarea);
                Console.WriteLine("Descripción: " + tarea.DescripcionTarea);
                Console.WriteLine("Prioridad: " + prioridadTexto);
            }
        }
        else
        {
            Console.WriteLine("\nNo se encontraron tareas de ese tipo");
        }

    }

    static void ElminarTarea(){ 

    Console.WriteLine("Ingrese el id de la tarea a eliminar: ");

    if(int.TryParse(Console.ReadLine(), out int idTarea))
    {
        var tareaAEliminar = tareas.FirstOrDefault(t => t.IdTarea == idTarea);
        //FirstOrDefault busca el primer elemento que cumpla con la condicion 
        //t => t.IdTarea == idTarea pregunta si el id de la tarea es igual al id que se ingreso

        if (tareaAEliminar != null)
        {
            tareas.Remove(tareaAEliminar);
            Console.WriteLine("Tarea: "+ idTarea +" eliminada con éxito");
        }
        else
        {
            Console.WriteLine("No se encontró una tarea con ese Id");
        }
    }
        else
        {
            Console.WriteLine("Id inválido. Por favor, ingrese un número válido.");
            Console.ReadKey();


        }
        Console.WriteLine("\nPresione cualquier tecla para volver al menú:");
        Console.ReadKey();

    }

    static void ExportarTareas(){

        string rutaArchivo =@"C:\Users\Navar\Desktop\tareas\tareas.txt"; //ruta donde se guardara el archivo de texto
        Directory.CreateDirectory(Path.GetDirectoryName(rutaArchivo)); //crea la carpeta si no existe
            using (StreamWriter writer = new StreamWriter(rutaArchivo)) 
            {
                foreach (var tarea in tareas)
                {
                    //string prioridadTexto = tarea.PrioridadTarea == true ? "Alta" : "Baja"; 
                    writer.WriteLine(tarea.IdTarea + "," + tarea.NombreTarea + "," + tarea.DescripcionTarea + "," + tarea.Tipotarea + "," + tarea.PrioridadTarea); 
                }
        } 
       
        Console.WriteLine("Tareas exportadas a tareas.txt");
        Console.WriteLine("\nPresione cualquier tecla para volver al menú:");
        Console.ReadKey();
    }

    static void ImportarTareas(List<Tarea> listaTareas)
    {
        string rutaArchivo = @"C:\Users\Navar\Desktop\tareas\tareas.txt";

        if (!File.Exists(rutaArchivo)) //aseguro que el archivo existe 
        {
            Console.WriteLine("El archivo no existe.");
            return;  
        }

        string[] lineas = File.ReadAllLines(rutaArchivo); //para leer todas las líneas del archivo y guardarlas en un array de string

        foreach (string linea in lineas) 
        {
            if (listaTareas.Count >= 4)
                break; // Se para cuando haya 4 tareas importadas

            if (string.IsNullOrWhiteSpace(linea)) //para evitar que lea lineas vacias   
                continue; 

            string[] datos = linea.Split(','); //Split para separar los datos por comas

            if (datos.Length != 5) //aseguro que la linea tenga los 5 elementos del constructor 
            {
                Console.WriteLine("Línea inválida (se esperaban 5 campos): " + linea);
                continue;
            }

            try
            {
                int idContador = int.Parse(datos[0]); // Id de la tarea
                string nombre = datos[1];   // Nombre de la tarea
                string descripcionTarea = datos[2];     // Descripción de la tarea
                TipoTarea tipo = (TipoTarea)Enum.Parse(typeof(TipoTarea), datos[3]); // Tipo de tarea (Persona, Trabajo u Ocio)
                bool prioridad = bool.Parse(datos[4]);     // prioridad (true o false)

                Tarea tarea = new Tarea(idContador, nombre, descripcionTarea, tipo, prioridad); //creo la tarea con el constructor
                listaTareas.Add(tarea);
            }
            catch (Exception)
            {
                Console.WriteLine("Error al importar la línea: " + linea);

            }
        }

        Console.WriteLine("Tareas importadas correctamente.");
        Console.WriteLine("\nPresione cualquier tecla para volver al menú:");
        Console.ReadKey();
    }
}

