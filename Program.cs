using System.Globalization;

const string UsersFilePath = "Users.txt";
const string PeopleFilePath = "People.txt";
const string LogFilePath = "log.txt";
const int MaxFailedAttempts = 3;

EnsureInitialFiles();
ShowTitle();

User? loggedUser = ValidateLogin();

if (loggedUser != null)
{
    Console.WriteLine();
    Console.WriteLine($"Inicio de sesión exitoso. Bienvenido, {loggedUser.Username}");
    ShowMainMenu(loggedUser);
}

Console.WriteLine();
Console.WriteLine("Presione ENTER para salir...");
Console.ReadLine();

void ShowTitle()
{
    Console.WriteLine("==========================================");
    Console.WriteLine("Taller #6 - Archivos Planos");
    Console.WriteLine("==========================================");
}

void EnsureInitialFiles()
{
    try
    {
        if (!File.Exists(UsersFilePath))
        {
            File.WriteAllLines(UsersFilePath, new[]
            {
                "admin,Admin123*,true",
                "jzuluaga,P@ssw0rd123!,true",
                "mbedoya,S0yS3gur02025*,true"
            });
        }

        if (!File.Exists(PeopleFilePath))
        {
            File.WriteAllLines(PeopleFilePath, new[]
            {
                "1,Maria,Bedoya,3223114015,Medellin,15000",
                "2,Juan,Zuluaga,3223114620,Medellin,8200",
                "3,Brad,Pit,3224504545,Miami,14000000"
            });
        }

        if (!File.Exists(LogFilePath))
        {
            File.WriteAllText(LogFilePath, string.Empty);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al preparar los archivos iniciales: {ex.Message}");
    }
}

List<User> ReadUsers()
{
    List<User> users = new();

    try
    {
        if (!File.Exists(UsersFilePath))
        {
            return users;
        }

        string[] lines = File.ReadAllLines(UsersFilePath);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] parts = line.Split(',');

            if (parts.Length != 3)
            {
                RegisterAction("SISTEMA", $"Línea omitida en Users.txt ({i + 1}): cantidad de campos inválida");
                continue;
            }

            if (!bool.TryParse(parts[2].Trim(), out bool isActive))
            {
                RegisterAction("SISTEMA", $"Línea omitida en Users.txt ({i + 1}): campo activo inválido");
                continue;
            }

            users.Add(new User
            {
                Username = parts[0].Trim(),
                Password = parts[1].Trim(),
                IsActive = isActive
            });
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al leer Users.txt: {ex.Message}");
        RegisterAction("SISTEMA", $"Error al leer Users.txt: {ex.Message}");
    }

    return users;
}

bool SaveUsers(List<User> users)
{
    try
    {
        List<string> lines = new();

        foreach (User user in users)
        {
            lines.Add($"{user.Username},{user.Password},{user.IsActive.ToString().ToLower(CultureInfo.InvariantCulture)}");
        }

        File.WriteAllLines(UsersFilePath, lines);
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al guardar Users.txt: {ex.Message}");
        RegisterAction("SISTEMA", $"Error al guardar Users.txt: {ex.Message}");
        return false;
    }
}

void RegisterAction(string username, string action)
{
    try
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logLine = $"{date} | Usuario: {username} | Acción: {action}";

        File.AppendAllText(LogFilePath, logLine + Environment.NewLine);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"No se pudo escribir en log.txt: {ex.Message}");
    }
}

List<Person> ReadPeople()
{
    List<Person> people = new();

    try
    {
        if (!File.Exists(PeopleFilePath))
        {
            return people;
        }

        string[] lines = File.ReadAllLines(PeopleFilePath);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] parts = line.Split(',');

            if (parts.Length != 6)
            {
                RegisterAction("SISTEMA", $"Línea omitida en People.txt ({i + 1}): cantidad de campos inválida");
                continue;
            }

            if (!int.TryParse(parts[0].Trim(), out int id))
            {
                RegisterAction("SISTEMA", $"Línea omitida en People.txt ({i + 1}): ID inválido");
                continue;
            }

            if (!decimal.TryParse(parts[5].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal balance))
            {
                RegisterAction("SISTEMA", $"Línea omitida en People.txt ({i + 1}): balance inválido");
                continue;
            }

            people.Add(new Person
            {
                Id = id,
                FirstName = parts[1].Trim(),
                LastName = parts[2].Trim(),
                Phone = parts[3].Trim(),
                City = parts[4].Trim(),
                Balance = balance
            });
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al leer People.txt: {ex.Message}");
        RegisterAction("SISTEMA", $"Error al leer People.txt: {ex.Message}");
    }

    return people;
}

bool SavePeople(List<Person> people)
{
    try
    {
        List<string> lines = new();

        foreach (Person person in people)
        {
            lines.Add($"{person.Id},{person.FirstName},{person.LastName},{person.Phone},{person.City},{person.Balance.ToString(CultureInfo.InvariantCulture)}");
        }

        File.WriteAllLines(PeopleFilePath, lines);
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al guardar People.txt: {ex.Message}");
        RegisterAction("SISTEMA", $"Error al guardar People.txt: {ex.Message}");
        return false;
    }
}

void ShowPerson(Person person)
{
    Console.WriteLine($"{person.Id}    {person.FirstName} {person.LastName}");
    Console.WriteLine($"     Phone: {person.Phone}");
    Console.WriteLine($"     City: {person.City}");
    Console.WriteLine($"     Balance: {person.Balance.ToString("C2", CultureInfo.GetCultureInfo("en-US"))}");
    Console.WriteLine();
}

void ShowAllPeople(List<Person> people, string username)
{
    Console.WriteLine();
    Console.WriteLine("Listado de personas");
    Console.WriteLine("------------------------------------------");

    if (people.Count == 0)
    {
        Console.WriteLine("No hay personas registradas.");
    }
    else
    {
        foreach (Person person in people)
        {
            ShowPerson(person);
        }
    }

    RegisterAction(username, "Consultó el listado de personas");
}

void ShowMainMenu(User loggedUser)
{
    bool exit = false;
    bool hasUnsavedChanges = false;
    List<Person> people = ReadPeople();

    while (!exit)
    {
        Console.WriteLine();
        Console.WriteLine("==========================================");
        Console.WriteLine("1. Mostrar personas");
        Console.WriteLine("2. Agregar persona");
        Console.WriteLine("3. Editar persona");
        Console.WriteLine("4. Eliminar persona");
        Console.WriteLine("5. Informe por ciudad");
        Console.WriteLine("6. Guardar cambios");
        Console.WriteLine("0. Salir");
        Console.WriteLine("==========================================");
        Console.Write("Elija una opción: ");

        string option = Console.ReadLine()?.Trim() ?? string.Empty;
        exit = ProcessMenuOption(option, loggedUser, people, ref hasUnsavedChanges);
    }
}

bool ProcessMenuOption(string option, User loggedUser, List<Person> people, ref bool hasUnsavedChanges)
{
    switch (option)
    {
        case "1":
            ShowAllPeople(people, loggedUser.Username);
            return false;

        case "2":
            if (AddPerson(people, loggedUser.Username))
            {
                hasUnsavedChanges = true;
            }

            return false;

        case "3":
            if (EditPerson(people, loggedUser.Username))
            {
                hasUnsavedChanges = true;
            }

            return false;

        case "4":
            if (DeletePerson(people, loggedUser.Username))
            {
                hasUnsavedChanges = true;
            }

            return false;

        case "5":
            ShowCityReport(people, loggedUser.Username);
            return false;

        case "6":
            if (SavePeople(people))
            {
                hasUnsavedChanges = false;
                Console.WriteLine("Cambios guardados correctamente.");
                RegisterAction(loggedUser.Username, "Guardó cambios en People.txt");
            }

            return false;

        case "0":
            if (hasUnsavedChanges)
            {
                Console.WriteLine("Hay cambios no guardados. Use la opción 6 para guardarlos antes de salir.");
            }

            Console.WriteLine("Saliendo del sistema...");
            RegisterAction(loggedUser.Username, "Salió del sistema");
            return true;

        default:
            Console.WriteLine("Opción inválida.");
            RegisterAction(loggedUser.Username, $"Ingresó una opción inválida: {option}");
            return false;
    }
}

bool AddPerson(List<Person> people, string username)
{
    Console.WriteLine();
    Console.WriteLine("Agregar nueva persona");

    int id = ReadValidId(people);
    string firstName = ReadRequiredText("Nombres: ", "Los nombres son obligatorios.");
    string lastName = ReadRequiredText("Apellidos: ", "Los apellidos son obligatorios.");
    string phone = ReadValidPhone();
    string city = ReadRequiredText("Ciudad: ", "La ciudad es obligatoria.");
    decimal balance = ReadValidBalance();

    Person person = new()
    {
        Id = id,
        FirstName = firstName,
        LastName = lastName,
        Phone = phone,
        City = city,
        Balance = balance
    };

    people.Add(person);
    Console.WriteLine("Persona agregada correctamente.");
    RegisterAction(username, $"Agregó persona ID {id}");
    return true;
}

bool EditPerson(List<Person> people, string username)
{
    Console.WriteLine();
    Console.WriteLine("Editar persona");

    int id = ReadNumericId("ID de la persona a editar: ");
    Person? person = people.FirstOrDefault(person => person.Id == id);

    if (person == null)
    {
        Console.WriteLine("No existe una persona con ese ID.");
        RegisterAction(username, $"Intentó editar persona inexistente ID {id}");
        return false;
    }

    Console.WriteLine();
    Console.WriteLine("Datos actuales:");
    ShowPerson(person);

    person.FirstName = ReadOptionalText("Nuevos nombres: ", person.FirstName, "Los nombres no pueden quedar vacíos.");
    person.LastName = ReadOptionalText("Nuevos apellidos: ", person.LastName, "Los apellidos no pueden quedar vacíos.");
    person.Phone = ReadOptionalPhone(person.Phone);
    person.City = ReadOptionalText("Nueva ciudad: ", person.City, "La ciudad no puede quedar vacía.");
    person.Balance = ReadOptionalBalance(person.Balance);

    Console.WriteLine("Persona actualizada correctamente.");
    RegisterAction(username, $"Editó persona ID {id}");
    return true;
}

bool DeletePerson(List<Person> people, string username)
{
    Console.WriteLine();
    Console.WriteLine("Eliminar persona");

    int id = ReadNumericId("ID de la persona a eliminar: ");
    Person? person = people.FirstOrDefault(person => person.Id == id);

    if (person == null)
    {
        Console.WriteLine("No existe una persona con ese ID.");
        RegisterAction(username, $"Intentó eliminar persona inexistente ID {id}");
        return false;
    }

    Console.WriteLine();
    Console.WriteLine("Datos actuales:");
    ShowPerson(person);

    while (true)
    {
        Console.Write("¿Está seguro de eliminar esta persona? (S/N): ");
        string confirmation = Console.ReadLine()?.Trim() ?? string.Empty;

        if (confirmation.Equals("S", StringComparison.OrdinalIgnoreCase))
        {
            people.Remove(person);
            Console.WriteLine("Persona eliminada correctamente.");
            RegisterAction(username, $"Eliminó persona ID {id}");
            return true;
        }

        if (confirmation.Equals("N", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Eliminación cancelada.");
            RegisterAction(username, $"Canceló eliminación de persona ID {id}");
            return false;
        }

        Console.WriteLine("Opción inválida. Responda S o N.");
    }
}

void ShowCityReport(List<Person> people, string username)
{
    Console.WriteLine();
    Console.WriteLine("Informe por ciudad");
    Console.WriteLine("==========================================");

    if (people.Count == 0)
    {
        Console.WriteLine("No hay personas registradas para generar el informe.");
        RegisterAction(username, "Consultó informe por ciudad sin datos");
        return;
    }

    decimal grandTotal = 0;

    var cityGroups = people
        .GroupBy(person => person.City.Trim().ToUpperInvariant())
        .OrderBy(group => group.First().City);

    foreach (var cityGroup in cityGroups)
    {
        string cityName = cityGroup.First().City.Trim();
        decimal cityTotal = cityGroup.Sum(person => person.Balance);
        grandTotal += cityTotal;

        Console.WriteLine();
        Console.WriteLine($"Ciudad: {cityName}");
        Console.WriteLine();
        Console.WriteLine($"{"ID",-5} {"Nombres",-20} {"Apellidos",-20} {"Balance",15}");
        Console.WriteLine("--------------------------------------------------------");

        foreach (Person person in cityGroup.OrderBy(person => person.Id))
        {
            string balance = person.Balance.ToString("C2", CultureInfo.GetCultureInfo("en-US"));
            Console.WriteLine($"{person.Id,-5} {person.FirstName,-20} {person.LastName,-20} {balance,15}");
        }

        Console.WriteLine();
        Console.WriteLine($"Total {cityName}: {cityTotal.ToString("C2", CultureInfo.GetCultureInfo("en-US"))}");
    }

    Console.WriteLine();
    Console.WriteLine($"Total general: {grandTotal.ToString("C2", CultureInfo.GetCultureInfo("en-US"))}");
    RegisterAction(username, "Consultó informe por ciudad");
}

int ReadNumericId(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string input = Console.ReadLine()?.Trim() ?? string.Empty;

        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("El ID debe ser numérico.");
            continue;
        }

        return id;
    }
}

int ReadValidId(List<Person> people)
{
    while (true)
    {
        Console.Write("ID: ");
        string input = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("El ID es obligatorio.");
            continue;
        }

        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("El ID debe ser numérico.");
            continue;
        }

        if (id <= 0)
        {
            Console.WriteLine("El ID debe ser positivo.");
            continue;
        }

        if (people.Any(person => person.Id == id))
        {
            Console.WriteLine("El ID ya existe. Ingrese un ID diferente.");
            continue;
        }

        return id;
    }
}

string ReadRequiredText(string prompt, string errorMessage)
{
    while (true)
    {
        Console.Write(prompt);
        string input = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine(errorMessage);
            continue;
        }

        return input;
    }
}

string ReadOptionalText(string prompt, string currentValue, string errorMessage)
{
    while (true)
    {
        Console.Write(prompt);
        string input = Console.ReadLine() ?? string.Empty;

        if (input == string.Empty)
        {
            return currentValue;
        }

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine(errorMessage);
            continue;
        }

        return input.Trim();
    }
}

string ReadValidPhone()
{
    while (true)
    {
        Console.Write("Teléfono: ");
        string phone = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(phone))
        {
            Console.WriteLine("El teléfono es obligatorio.");
            continue;
        }

        if (!IsValidColombianMobilePhone(phone))
        {
            Console.WriteLine("El teléfono debe ser un celular colombiano válido de 10 dígitos.");
            continue;
        }

        return phone;
    }
}

string ReadOptionalPhone(string currentPhone)
{
    while (true)
    {
        Console.Write("Nuevo teléfono: ");
        string phone = Console.ReadLine() ?? string.Empty;

        if (phone == string.Empty)
        {
            return currentPhone;
        }

        phone = phone.Trim();

        if (!IsValidColombianMobilePhone(phone))
        {
            Console.WriteLine("El teléfono debe ser un celular colombiano válido de 10 dígitos.");
            continue;
        }

        return phone;
    }
}

bool IsValidColombianMobilePhone(string phone)
{
    string[] validPrefixes =
    {
        "300", "301", "302", "303", "304", "305",
        "310", "311", "312", "313", "314", "315", "316", "317", "318", "319",
        "320", "321", "322", "323",
        "333",
        "350", "351"
    };

    return phone.Length == 10
        && phone.All(char.IsDigit)
        && validPrefixes.Any(prefix => phone.StartsWith(prefix));
}

decimal ReadValidBalance()
{
    while (true)
    {
        Console.Write("Balance: ");
        string input = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("El balance es obligatorio.");
            continue;
        }

        if (!decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal balance))
        {
            Console.WriteLine("El balance debe ser numérico.");
            continue;
        }

        if (balance < 0)
        {
            Console.WriteLine("El balance debe ser positivo o cero.");
            continue;
        }

        return balance;
    }
}

decimal ReadOptionalBalance(decimal currentBalance)
{
    while (true)
    {
        Console.Write("Nuevo balance: ");
        string input = Console.ReadLine() ?? string.Empty;

        if (input == string.Empty)
        {
            return currentBalance;
        }

        input = input.Trim();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("El balance no puede quedar vacío.");
            continue;
        }

        if (!decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal balance))
        {
            Console.WriteLine("El balance debe ser numérico.");
            continue;
        }

        if (balance < 0)
        {
            Console.WriteLine("El balance debe ser positivo o cero.");
            continue;
        }

        return balance;
    }
}

User? ValidateLogin()
{
    int failedAttempts = 0;
    string? currentUsername = null;

    while (true)
    {
        Console.Write("Usuario: ");
        string username = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Contraseña: ");
        string password = Console.ReadLine() ?? string.Empty;

        List<User> users = ReadUsers();
        User? user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (user == null)
        {
            Console.WriteLine("Error: el usuario no existe.");
            RegisterAction(username, "Intento fallido de inicio de sesión - usuario no existe");
            continue;
        }

        if (!user.IsActive)
        {
            Console.WriteLine("Error: el usuario está bloqueado o inactivo.");
            RegisterAction(user.Username, "Intento fallido de inicio de sesión - usuario bloqueado o inactivo");
            continue;
        }

        if (currentUsername == null || !currentUsername.Equals(user.Username, StringComparison.OrdinalIgnoreCase))
        {
            currentUsername = user.Username;
            failedAttempts = 0;
        }

        if (user.Password == password)
        {
            RegisterAction(user.Username, "Inicio de sesión exitoso");
            return user;
        }

        failedAttempts++;
        Console.WriteLine($"Error: contraseña incorrecta. Intento {failedAttempts} de {MaxFailedAttempts}.");
        RegisterAction(user.Username, "Intento fallido de inicio de sesión");

        if (failedAttempts >= MaxFailedAttempts)
        {
            user.IsActive = false;
            SaveUsers(users);
            Console.WriteLine("El usuario fue bloqueado por 3 intentos fallidos.");
            RegisterAction(user.Username, "Usuario bloqueado por 3 intentos fallidos");
            return null;
        }
    }
}

class User
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}
