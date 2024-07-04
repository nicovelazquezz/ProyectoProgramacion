namespace ProyectoCRUD
{
    public class Program
    {
        // creación de estructuras de alumnos, materias y alumnos-materias
        public struct Alumno
        {
            public int Indice;
            public string Nombre;
            public string Apellido;
            public string DNI;
            public string FechaNacimiento;
            public string Domicilio;
            public bool EstaActivo;
        }

        public struct Materia
        {
            public int Indice;
            public string NombreMateria;
            public bool EstaActiva;
        }

        public struct AlumnoMateria
        {
            public int Indice;
            public int IndiceAlumno;
            public int IndiceMateria;
            public string Estado;
            public int Nota;
            public string Fecha;
        }

        // método de ordenamiento burbuja para alumnos, autoejecutable cada vez que se modifica la lista de alumnos.
        public static void OrdenarAlumnosPorDNI(ref List<Alumno> alumnos)
        {
            int n = alumnos.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (string.Compare(alumnos[j].DNI, alumnos[j + 1].DNI) > 0)
                    {
                        Alumno temp = alumnos[j];
                        alumnos[j] = alumnos[j + 1];
                        alumnos[j + 1] = temp;
                    }
                }
            }
        }

        // funciones de gestión de alumnos
        public static void AgregarAlumno(ref List<Alumno> alumnos, Alumno nuevoAlumno)
        {
            var alumnoExistente = alumnos.FirstOrDefault(a => a.DNI == nuevoAlumno.DNI);
            if (!string.IsNullOrEmpty(alumnoExistente.DNI))
            {
                if (alumnoExistente.EstaActivo)
                {
                    Console.WriteLine("Ya existe un alumno activo con el mismo DNI.");
                    return;
                }
                else
                {
                    Console.WriteLine("El alumno con este DNI está desactivado. ¿Desea reactivarlo? (s/n)");
                    if (Console.ReadLine().ToLower() == "s")
                    {
                        var index = alumnos.FindIndex(a => a.DNI == nuevoAlumno.DNI);
                        if (index != -1)
                        {
                            var alumnoModificado = alumnos[index];
                            alumnoModificado.EstaActivo = true;
                            alumnos[index] = alumnoModificado;
                            Persistencia.GuardarAlumnos(alumnos, "alumnos.txt");
                            Console.WriteLine("Alumno reactivado.");
                        }
                    }
                    return;
                }
            }

            nuevoAlumno.Indice = alumnos.Count > 0 ? alumnos.Max(a => a.Indice) + 1 : 1;
            alumnos.Add(nuevoAlumno);
            OrdenarAlumnosPorDNI(ref alumnos); // al agregar alumnos ordenar la lista de alumnos por DNI
            Persistencia.GuardarAlumnos(alumnos, "alumnos.txt");
            Console.WriteLine("Alumno agregado.");
        }

        public static void MostrarAlumnos(List<Alumno> alumnos, bool activos)
        {
            var alumnosFiltrados = alumnos.Where(a => a.EstaActivo == activos).ToList();
            foreach (var alumno in alumnosFiltrados)
            {
                Console.WriteLine($"{alumno.Indice}, {alumno.Nombre}, {alumno.Apellido}, {alumno.DNI}, {alumno.FechaNacimiento}, {alumno.Domicilio}, {alumno.EstaActivo}");
            }
        }

        public static void ModificarAlumno(ref List<Alumno> alumnos, string dni)
        {
            var alumnoIndex = alumnos.FindIndex(a => a.DNI == dni);
            if (alumnoIndex == -1)
            {
                Console.WriteLine("Alumno no encontrado.");
                return;
            }

            var alumno = alumnos[alumnoIndex];

            Console.Write("Nombre: ");
            alumno.Nombre = Console.ReadLine();
            Console.Write("Apellido: ");
            alumno.Apellido = Console.ReadLine();
            Console.Write("Fecha de Nacimiento (dd/mm/yyyy): ");
            alumno.FechaNacimiento = Console.ReadLine();
            Console.Write("Domicilio: ");
            alumno.Domicilio = Console.ReadLine();
            alumno.EstaActivo = true; // Estado siempre activo en la modificación

            alumnos[alumnoIndex] = alumno;
            OrdenarAlumnosPorDNI(ref alumnos); // Ordenar la lista de alumnos por DNI
            Persistencia.GuardarAlumnos(alumnos, "alumnos.txt");
            Console.WriteLine("Alumno modificado.");
        }

        public static void BajaAlumno(ref List<Alumno> alumnos, string dni)
        {
            var alumnoIndex = alumnos.FindIndex(a => a.DNI == dni);
            if (alumnoIndex == -1)
            {
                Console.WriteLine("Alumno no encontrado.");
                return;
            }

            var alumno = alumnos[alumnoIndex];

            Console.WriteLine("¿Está seguro de que desea desactivar este alumno? (s/n)");
            if (Console.ReadLine().ToLower() == "s")
            {
                alumno.EstaActivo = false;
                alumnos[alumnoIndex] = alumno;
                OrdenarAlumnosPorDNI(ref alumnos); // Ordenar la lista de alumnos por DNI
                Persistencia.GuardarAlumnos(alumnos, "alumnos.txt");
                Console.WriteLine("Alumno desactivado.");
            }
        }

        // Gestión de materias
        public static void AgregarMateria(ref List<Materia> materias, Materia nuevaMateria)
        {
            if (materias.Any(m => m.NombreMateria == nuevaMateria.NombreMateria))
            {
                Console.WriteLine("Ya existe una materia con el mismo nombre.");
                return;
            }

            nuevaMateria.Indice = materias.Count > 0 ? materias.Max(m => m.Indice) + 1 : 1;
            nuevaMateria.EstaActiva = true; // Materia siempre activa al agregar
            materias.Add(nuevaMateria);
            Persistencia.GuardarMaterias(materias, "materias.txt");
            Console.WriteLine("Materia agregada.");
        }

        public static void ModificarMateria(ref List<Materia> materias, int indice)
        {
            var materiaIndex = materias.FindIndex(m => m.Indice == indice);
            if (materiaIndex == -1)
            {
                Console.WriteLine("Materia no encontrada.");
                return;
            }

            var materia = materias[materiaIndex];

            Console.Write("Nombre de la Materia: ");
            materia.NombreMateria = Console.ReadLine();
            materia.EstaActiva = true; // Estado siempre activo en la modificación

            materias[materiaIndex] = materia;
            Persistencia.GuardarMaterias(materias, "materias.txt");
            Console.WriteLine("Materia modificada.");
        }

        public static void EliminarMateria(ref List<Materia> materias, int indice)
        {
            var materiaIndex = materias.FindIndex(m => m.Indice == indice);
            if (materiaIndex == -1)
            {
                Console.WriteLine("Materia no encontrada.");
                return;
            }

            var materia = materias[materiaIndex];

            Console.WriteLine("¿Está seguro de que desea desactivar esta materia? (s/n)");
            if (Console.ReadLine().ToLower() == "s")
            {
                materia.EstaActiva = false;
                materias[materiaIndex] = materia;
                Persistencia.GuardarMaterias(materias, "materias.txt");
                Console.WriteLine("Materia desactivada.");
            }
        }

        public static void MostrarMaterias(List<Materia> materias, bool activas)
        {
            var materiasFiltradas = materias.Where(m => m.EstaActiva == activas).ToList();
            foreach (var materia in materiasFiltradas)
            {
                Console.WriteLine($"{materia.Indice}, {materia.NombreMateria}, {materia.EstaActiva}");
            }
        }

        // Gestión de notas de materia
        public static void InscribirAlumno(ref List<AlumnoMateria> alumnoMaterias, AlumnoMateria nuevaInscripcion)
        {
            nuevaInscripcion.Indice = alumnoMaterias.Count > 0 ? alumnoMaterias.Max(am => am.Indice) + 1 : 1;
            alumnoMaterias.Add(nuevaInscripcion);
            Persistencia.GuardarAlumnoMaterias(alumnoMaterias, "alumnoMaterias.txt");
            Console.WriteLine("Alumno inscrito en la materia.");
        }

        public static void MostrarInscripciones(List<AlumnoMateria> alumnoMaterias)
        {
            foreach (var inscripcion in alumnoMaterias)
            {
                Console.WriteLine($"{inscripcion.Indice}, {inscripcion.IndiceAlumno}, {inscripcion.IndiceMateria}, {inscripcion.Estado}, {inscripcion.Nota}, {inscripcion.Fecha}");
            }
        }

        // Persistencia de datos en local
        public static class Persistencia
        {
            public static void GuardarAlumnos(List<Alumno> alumnos, string path)
            {
                using (StreamWriter escritor = new StreamWriter(path))
                {
                    foreach (var alumno in alumnos)
                    {
                        escritor.WriteLine($"{alumno.Indice},{alumno.Nombre},{alumno.Apellido},{alumno.DNI},{alumno.FechaNacimiento},{alumno.Domicilio},{alumno.EstaActivo}");
                    }
                }
            }

            public static List<Alumno> CargarAlumnos(string path)
            {
                List<Alumno> alumnos = new List<Alumno>();
                if (File.Exists(path))
                {
                    using (StreamReader file = new StreamReader(path))
                    {
                        string linea;
                        while ((linea = file.ReadLine()) != null)
                        {
                            string[] datos = linea.Split(',');
                            alumnos.Add(new Alumno
                            {
                                Indice = int.Parse(datos[0]),
                                Nombre = datos[1],
                                Apellido = datos[2],
                                DNI = datos[3],
                                FechaNacimiento = datos[4],
                                Domicilio = datos[5],
                                EstaActivo = bool.Parse(datos[6])
                            });
                        }
                    }
                }
                return alumnos;
            }

            public static void GuardarMaterias(List<Materia> materias, string path)
            {
                using (StreamWriter escritor = new StreamWriter(path))
                {
                    foreach (var materia in materias)
                    {
                        escritor.WriteLine($"{materia.Indice},{materia.NombreMateria},{materia.EstaActiva}");
                    }
                }
            }

            public static List<Materia> CargarMaterias(string path)
            {
                List<Materia> materias = new List<Materia>();
                if (File.Exists(path))
                {
                    using (StreamReader file = new StreamReader(path))
                    {
                        string linea;
                        while ((linea = file.ReadLine()) != null)
                        {
                            string[] datos = linea.Split(',');
                            materias.Add(new Materia
                            {
                                Indice = int.Parse(datos[0]),
                                NombreMateria = datos[1],
                                EstaActiva = bool.Parse(datos[2])
                            });
                        }
                    }
                }
                return materias;
            }

            public static void GuardarAlumnoMaterias(List<AlumnoMateria> alumnoMaterias, string path)
            {
                using (StreamWriter escritor = new StreamWriter(path))
                {
                    foreach (var inscripcion in alumnoMaterias)
                    {
                        escritor.WriteLine($"{inscripcion.Indice},{inscripcion.IndiceAlumno},{inscripcion.IndiceMateria},{inscripcion.Estado},{inscripcion.Nota},{inscripcion.Fecha}");
                    }
                }
            }

            public static List<AlumnoMateria> CargarAlumnoMaterias(string path)
            {
                List<AlumnoMateria> alumnoMaterias = new List<AlumnoMateria>();
                if (File.Exists(path))
                {
                    // Leer archivo y cargar datos
                    using (StreamReader file = new StreamReader(path))
                    {
                        string linea;
                        while ((linea = file.ReadLine()) != null)
                        {
                            string[] datos = linea.Split(',');
                            alumnoMaterias.Add(new AlumnoMateria
                            {
                                Indice = int.Parse(datos[0]),
                                IndiceAlumno = int.Parse(datos[1]),
                                IndiceMateria = int.Parse(datos[2]),
                                Estado = datos[3],
                                Nota = int.Parse(datos[4]),
                                Fecha = datos[5]
                            });
                        }
                    }
                }
                return alumnoMaterias;
            }
        }

        public static void MenuGestionAlumnos(ref List<Alumno> alumnos)
        {
            while (true)
            {
                Console.WriteLine("\nGestión de Alumnos");
                Console.WriteLine("1. Agregar Alumno");
                Console.WriteLine("2. Mostrar Alumnos Activos");
                Console.WriteLine("3. Modificar Alumno");
                Console.WriteLine("4. Baja de Alumno");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Alumno nuevoAlumno = new Alumno();

                        Console.Write("Nombre: ");
                        nuevoAlumno.Nombre = Console.ReadLine();
                        Console.Write("Apellido: ");
                        nuevoAlumno.Apellido = Console.ReadLine();
                        Console.Write("DNI: ");
                        nuevoAlumno.DNI = Console.ReadLine();
                        Console.Write("Fecha de Nacimiento (dd/mm/yyyy): ");
                        nuevoAlumno.FechaNacimiento = Console.ReadLine();
                        Console.Write("Domicilio: ");
                        nuevoAlumno.Domicilio = Console.ReadLine();
                        nuevoAlumno.EstaActivo = true;

                        AgregarAlumno(ref alumnos, nuevoAlumno);
                        break;

                    case "2":
                        Console.WriteLine("Alumnos Activos:");
                        MostrarAlumnos(alumnos, true);
                        break;

                    case "3":
                        Console.Write("DNI del alumno a modificar: ");
                        string dniModificar = Console.ReadLine();
                        ModificarAlumno(ref alumnos, dniModificar);
                        break;

                    case "4":
                        Console.Write("DNI del alumno a desactivar: ");
                        string dniDesactivar = Console.ReadLine();
                        BajaAlumno(ref alumnos, dniDesactivar);
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Opción no válida. Intente nuevamente.");
                        break;
                }
            }
        }

        public static void Main(string[] args)
        {
            List<Alumno> alumnos = Persistencia.CargarAlumnos("alumnos.txt");
            List<Materia> materias = Persistencia.CargarMaterias("materias.txt");
            List<AlumnoMateria> alumnoMaterias = Persistencia.CargarAlumnoMaterias("alumnoMaterias.txt");

            while (true)
            {
                Console.WriteLine("\nMenú Principal");
                Console.WriteLine("1. Gestión de Alumnos");
                Console.WriteLine("2. Gestión de Materias");
                Console.WriteLine("3. Inscripción de Alumnos en Materias");
                Console.WriteLine("4. Mostrar Inscripciones");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MenuGestionAlumnos(ref alumnos);
                        break;

                    case "2":
                        MenuGestionMaterias(ref materias);
                        break;

                    case "3":
                        InscribirAlumnoEnMateria(ref alumnoMaterias, ref alumnos, ref materias);
                        break;

                    case "4":
                        MostrarInscripciones(alumnoMaterias);
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Opción no válida. Intente nuevamente.");
                        break;
                }
            }
        }

        // gestión de materias
        public static void MenuGestionMaterias(ref List<Materia> materias)
        {
            while (true)
            {
                Console.WriteLine("\nGestión de Materias");
                Console.WriteLine("1. Agregar Materia");
                Console.WriteLine("2. Mostrar Materias Activas");
                Console.WriteLine("3. Modificar Materia");
                Console.WriteLine("4. Eliminar Materia");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Materia nuevaMateria = new Materia();

                        Console.Write("Nombre de la Materia: ");
                        nuevaMateria.NombreMateria = Console.ReadLine();
                        nuevaMateria.EstaActiva = true;

                        AgregarMateria(ref materias, nuevaMateria);
                        break;

                    case "2":
                        Console.WriteLine("Materias Activas:");
                        MostrarMaterias(materias, true);
                        break;

                    case "3":
                        Console.Write("Índice de la materia a modificar: ");
                        int indiceModificar = int.Parse(Console.ReadLine());
                        ModificarMateria(ref materias, indiceModificar);
                        break;

                    case "4":
                        Console.Write("Índice de la materia a eliminar: ");
                        int indiceEliminar = int.Parse(Console.ReadLine());
                        EliminarMateria(ref materias, indiceEliminar);
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Opción no válida. Intente nuevamente.");
                        break;
                }
            }
        }

        // inscripción de alumnos en materias
        public static void InscribirAlumnoEnMateria(ref List<AlumnoMateria> alumnoMaterias, ref List<Alumno> alumnos, ref List<Materia> materias)
        {
            AlumnoMateria nuevaInscripcion = new AlumnoMateria();

            Console.Write("Índice del Alumno: ");
            nuevaInscripcion.IndiceAlumno = int.Parse(Console.ReadLine());
            Console.Write("Índice de la Materia: ");
            nuevaInscripcion.IndiceMateria = int.Parse(Console.ReadLine());
            nuevaInscripcion.Fecha = DateTime.Now.ToString("dd/MM/yyyy");

            Console.Write("Estado (Anotado, Cursado, Aprobado, Desaprobado): ");
            nuevaInscripcion.Estado = Console.ReadLine();
            Console.Write("Nota: ");
            nuevaInscripcion.Nota = int.Parse(Console.ReadLine());

            InscribirAlumno(ref alumnoMaterias, nuevaInscripcion);
        }
    }
}
