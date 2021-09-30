using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string ConString = "" + @"Data source=.\SQLEXPRESS; " + "Initial catalog=Person; " + "Trusted_Connection = True;";
            try
            {
                var enter = new Bank();
                enter.AdminEnter(ConString);
                while (true)
                {
                    Console.WriteLine("       1. вход в кабинет клиента 2. создать КЛИЕНТА 3. создать АДМИНА\n");
                    Console.Write("ваш выбор: ");
                    int choice = Convert.ToInt32(Console.ReadLine());
                    var ret = 0;
                    var login = "";
                    var clientId = 0;
                    var activecredit = 0;
                    switch (choice)
                    {
                        case 1:
                            for (int i = 1; i != 0; i++)
                            {
                                Console.WriteLine("                      Вход в систему\n");
                                Console.Write("      Логин: ");
                                login = Console.ReadLine();
                                Console.Clear();
                                var clientEnter = new Bank();
                                ret = clientEnter.ClientEnter(ConString, login);
                                if (ret == 1) i = -1;
                                var getId = new Bank();
                                clientId = getId.GetClientId(login, ConString);
                                var getactive = new Bank();
                                activecredit = getactive.GetCreditId(clientId, ConString);
                            }
                            for (int i = 1; i != 0; i++)
                            {
                                Console.WriteLine("      1. Добавить пасспорт и Форму  2. Сделать запрос на кредит  3. История моих кредитов");
                                var addChoice = Convert.ToInt32(Console.ReadLine());
                                switch (addChoice)
                                {
                                    case 1:
                                        var addPassport = new Bank();
                                        addPassport.AddPassport(ConString, clientId);
                                        if (addPassport.AddPassport(ConString, clientId) == 1)
                                        {
                                            i = -1;
                                            var addForm = new Bank();
                                            addForm.AddForm(ConString, clientId);
                                        }
                                        break;
                                    case 2:
                                        var application = new Bank();
                                        application.NewApplication(ConString, clientId);
                                        break;
                                    case 3:
                                        var myaccount = new Bank();
                                        myaccount.ClientAccount(ConString, clientId, activecredit);
                                        break;
                                }
                            }
                            break;
                        case 2:
                            var creatClient = new Bank();
                            creatClient.CreatClient(ConString);
                            break;
                        case 3:
                            var creatAdmin = new Bank();
                            creatAdmin.CreatAdmin(ConString);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    class Bank
    {
        public int ClientEnter(string ConString, string login)
        {
            var check = 0;
            try
            {
                var conn = new SqlConnection(ConString);
                conn.Open();
                var querylog = $"SELECT login FROM [dbo].[Clients] WHERE [login] = '{login}'";
                var com = conn.CreateCommand();
                com.CommandText = querylog;
                var reader = com.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine("                             Вход");
                    Console.Clear();
                    check = 1;
                }
                else Console.WriteLine("Не правельный логин");


                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return check;
        }
        public void AdminEnter(string ConString)
        {
            try
            {
                for (int i = 1; i != 0; i++)
                {
                    Console.WriteLine("                      Вход в систему\n");
                    Console.Write("      Логин: ");
                    var login = Console.ReadLine();
                    Console.Write("\n      Парол: ");
                    var pass = Console.ReadLine();
                    Console.Clear();
                    var connect = new SqlConnection(ConString);
                    connect.Open();
                    var querylog = $"SELECT login FROM [dbo].[Admins] WHERE [login] = '{login}'";
                    var querypas = $"SELECT password FROM [dbo].[Admins] WHERE [password] = '{pass}'";
                    var command = connect.CreateCommand();
                    command.CommandText = querylog;
                    command.CommandText = querypas;
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Console.WriteLine("                             Вход");
                        i = -1;
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("                      Неправильный логин или пароль");
                    }
                    reader.Close();
                    connect.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void CreatClient(string ConString)
        {
            try
            {
                Console.WriteLine("                      Введите номер телефон для создание нового клиента\n\n");
                Console.WriteLine();
                Console.Write("      Логин: ");
                var login = Console.ReadLine();
                var loginVal = login;
                Console.Clear();
                var conn = new SqlConnection(ConString);
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = $"SELECT Login FROM Clients WHERE login='{login}';";
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Console.Clear();
                    Console.WriteLine("                      Такой клиент уже есть в базе");
                }
                else
                {
                    reader.Close();
                    conn.Close();
                    var com = conn.CreateCommand();
                    com.CommandText = "INSERT INTO Clients([login]) VALUES (@login)";
                    com.Parameters.AddWithValue("@login", loginVal);
                    conn.Open();
                    var result = com.ExecuteNonQuery();
                    Console.Clear();
                    if (result > 0) Console.WriteLine("                      Новый клиент создан успешно");
                    conn.Close();
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public int GetClientId(string login, string ConString)
        {
            var accId = 0;
            try
            {
                var connection = new SqlConnection(ConString);
                connection.Open();
                var query = "SELECT [Id] FROM [dbo].[Clients] WHERE [login] = @number";
                var command = connection.CreateCommand();
                command.Parameters.AddWithValue("@number", login);
                command.CommandText = query;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    accId = reader.GetInt32(0);
                }
                connection.Close();
                reader.Close();
            }
            catch ( Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return accId;
        }
        public int GetCreditId(int clientId, string ConString)
        {
            var accId = 0;
            try
            {
                var connection = new SqlConnection(ConString);
                connection.Open();
                var query = "SELECT [Id] FROM [dbo].[Credit] WHERE [PersonId] = @clientId AND [ActiveNot] ='активно'";
                var command = connection.CreateCommand();
                command.Parameters.AddWithValue("@clientId", clientId);
                command.CommandText = query;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    accId = reader.GetInt32(0);
                }
                connection.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return accId;
        }
        public void CreatAdmin(string ConString)
        {
            try
            {
                Console.WriteLine("                     Создать нового админа\n");
                Console.Write("      придумайте логин: ");
                var login = Console.ReadLine();
                Console.Write("      придумайте пароль: ");
                var pass = Console.ReadLine();
                var conn = new SqlConnection(ConString);
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = $"SELECT Login FROM Admins WHERE login='{login}' AND password='{pass}';";
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Console.Clear();
                    Console.WriteLine("                      Такой админ уже есть в базе");
                }
                else
                {
                reader.Close();
                conn.Close();
                var com = conn.CreateCommand();
                com.CommandText = "INSERT INTO Admins([login], [password]) VALUES (@login, @pass)";
                com.Parameters.AddWithValue("@login", login);
                com.Parameters.AddWithValue("@pass", pass);
                conn.Open();
                var result = com.ExecuteNonQuery();
                Console.Clear();
                if (result > 0) Console.WriteLine("                      Новый админ создан успешно");
                conn.Close();
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public int AddPassport(string ConString, int clientId)
        {
            var check = 0;
            try
            {
                var conn = new SqlConnection(ConString);
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = $"SELECT * FROM Passport WHERE IdPerson={clientId};";
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine("                      Пасспорт уже был добавлен");
                    check = 0;
                }
                else
                {
                    reader.Close();
                    Console.WriteLine("                      Веедите данные паспорта");
                    Console.Write("      Имя: ");
                    var name = Console.ReadLine();
                    Console.Write("\n      Фамилия: ");
                    var lastName = Console.ReadLine();
                    Console.Write("\n      Отчество: ");
                    var midlName = Console.ReadLine();
                    Console.Write("\n      Дата рождения ПРИМЕР'9999/99/99 или 9999,99,99': ");
                    var birthDate = Console.ReadLine();
                    Console.Write("\n      Гражданство: ");
                    var citizen = Console.ReadLine();
                    Console.Write("\n      Пол: ");
                    var gender = Console.ReadLine();
                    Console.Write("\n      Номер наспорта: ");
                    var passportNuber = Console.ReadLine();
                    var bday = Convert.ToDateTime(birthDate);

                    var com = conn.CreateCommand();
                    com.CommandText = "INSERT INTO Passport([Name], [LastName], [MidlName], [birhtData], [citizen], [gender], [PasNumber], [IdPerson]) VALUES (@name, @lastName, @midlName, @bday, @citizen, @gender, @passportNuber, @clientId);";
                    com.Parameters.AddWithValue("@name", name);
                    com.Parameters.AddWithValue("@lastName", lastName);
                    com.Parameters.AddWithValue("@midlName", midlName);
                    com.Parameters.AddWithValue("@bday", bday);
                    com.Parameters.AddWithValue("@citizen", citizen);
                    com.Parameters.AddWithValue("@gender", gender);
                    com.Parameters.AddWithValue("@passportNuber", passportNuber);
                    com.Parameters.AddWithValue("@clientId", clientId);
                    var result = com.ExecuteNonQuery();
                    Console.Clear();
                    if (result > 0)
                    {
                        Console.WriteLine("                      Паспорт успешно добавлен");
                        check = 1;
                    }
                    conn.Close();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return check;
        }
        public void AddForm(string ConString, int clientId)
        {
            try
            {
                Console.Clear();
                var name = "";
                var lastname = "";
                var birhtData = "";
                var citizen = "";
                var pasnuber = "";
                var conn = new SqlConnection(ConString);
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = $"SELECT * FROM Passport WHERE PersonId={clientId};";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    name = !string.IsNullOrEmpty(reader.GetValue(1)?.ToString()) ? reader.GetString(1) : null;
                    lastname = !string.IsNullOrEmpty(reader.GetValue(2)?.ToString()) ? reader.GetString(2) : null;
                    birhtData = reader.GetDateTime(4).ToString();
                    citizen = !string.IsNullOrEmpty(reader.GetValue(6)?.ToString()) ? reader.GetString(6) : null;
                    pasnuber = !string.IsNullOrEmpty(reader.GetValue(10)?.ToString()) ? reader.GetString(10) : null;
                }
                reader.Close();
                conn.Close();

                Console.WriteLine("                      Заполните форму");
                Console.WriteLine($"\n      Имя: {name}");
                Console.WriteLine($"\n      Фамилия: {lastname}");
                Console.WriteLine($"\n      Дата рождения: {birhtData}");
                Console.WriteLine($"\n      Гражданство: {citizen}");
                Console.WriteLine($"\n      Номер Паспорта: {pasnuber}");
                Console.Write("\n      Семейное положение: ");
                var mStatus = Console.ReadLine();
                Console.Write("\n      Место жительство: ");
                var loc = Console.ReadLine();
                Console.Write("\n      Номер телефона: ");
                var phone = Console.ReadLine();
                Console.Write("\n      Адрес электронной почты: ");
                var mail = Console.ReadLine();

                var com = conn.CreateCommand();
                com.CommandText = "INSERT INTO Form([MStatus], [loc], [phone], [mail], [PersonId]) VALUES (@mStatus, @loc, @phone, @mail, 1);";
                com.Parameters.AddWithValue("@mStatus", mStatus);
                com.Parameters.AddWithValue("@loc", loc);
                com.Parameters.AddWithValue("@phone", phone);
                com.Parameters.AddWithValue("@mail", mail);
                conn.Open();
                var result = com.ExecuteNonQuery();
                Console.Clear();
                if (result > 0) Console.WriteLine("                      Форма успешно создана");
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void NewApplication(string ConString, int clientId)
        {
            try
            {
                var connection = new SqlConnection(ConString);
                connection.Open();
                var query = "SELECT [Id] FROM [dbo].[Credit] WHERE [PersonId] = @clientId AND [ActiveNot] ='активно'";
                var command = connection.CreateCommand();
                command.Parameters.AddWithValue("@clientId", clientId);
                command.CommandText = query;
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine("      У вас есть не непогашенный кредит: С начало закройте другие кредиты");
                }
                else
                {
                    var name = "";
                    var lastname = "";
                    var birhtData = "";
                    var citizen = "";
                    var pasnuber = "";
                    var gender = "";
                    var status = "";
                    var creditCount = 0;
                    var delayCount = 0;
                    var conn = new SqlConnection(ConString);
                    conn.Open();
                    var commands = conn.CreateCommand();
                    commands.CommandText = $"SELECT * FROM Passport WHERE IdPerson={clientId};";
                    var readers = commands.ExecuteReader();
                    while (readers.Read())
                    {
                        gender = !string.IsNullOrEmpty(readers.GetValue(6)?.ToString()) ? readers.GetString(6) : null;
                        name = !string.IsNullOrEmpty(readers.GetValue(1)?.ToString()) ? readers.GetString(1) : null;
                        lastname = !string.IsNullOrEmpty(readers.GetValue(2)?.ToString()) ? readers.GetString(2) : null;
                        birhtData = readers.GetDateTime(4).ToString();
                        citizen = !string.IsNullOrEmpty(readers.GetValue(5)?.ToString()) ? readers.GetString(5) : null;
                        pasnuber = !string.IsNullOrEmpty(readers.GetValue(7)?.ToString()) ? readers.GetString(7) : null;
                    }
                    readers.Close();
                    var command1 = conn.CreateCommand();
                    command1.CommandText = $"SELECT * FROM Form WHERE PersonId={clientId};";
                    var reader1 = command1.ExecuteReader();
                    while (reader1.Read())
                    {
                        status = !string.IsNullOrEmpty(reader1.GetValue(1)?.ToString()) ? reader1.GetString(1) : null;
                    }
                    reader1.Close();
                    var command2 = conn.CreateCommand();
                    command2.CommandText = $"SELECT COUNT(PersonId)FROM Credit WHERE PersonId={clientId};";
                    var reader2 = command2.ExecuteReader();
                    while (reader2.Read())
                    {
                        creditCount = reader2.GetInt32(0);
                    }
                    reader2.Close();
                    var command3 = conn.CreateCommand();
                    command3.CommandText = $"SELECT COUNT(UserId)FROM CreditHistory WHERE UserId={clientId} AND delay=1;";
                    var reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {
                        delayCount = reader3.GetInt32(0);
                    }
                    reader3.Close();
                    conn.Close();

                    Console.WriteLine("                   ЗАЯВЛЕНИЕ-АНКЕТА ФИЗИЧЕСКОГО ЛИЦА\n                         НА ПОЛУЧЕНИЕ КРЕДИТА");
                    Console.WriteLine("\n                  1. Запрашиваемые условия кредитования\n");
                    Console.Write($"      Цель кредитования: ");
                    var purp = Console.ReadLine();
                    var purpS = 0;
                    if (purp == "бытовая техника" || purp == "Бытовая техника") purpS = 2;
                    else if (purp == "ремонт" || purp == "Ремонт") purpS = 1;
                    else if (purp == "телефон" || purp == "Телефон") purpS = 0;
                    else if (purp == "прочее" || purp == "Прочее") purpS = -1;
                    else purpS = 0;
                    Console.Write("\n      Сумма кредита: ");
                    decimal creAmout = 0;
                    while (creAmout <= 500)
                    {
                        creAmout = Convert.ToDecimal(Console.ReadLine());
                        if (creAmout <= 500)
                        {
                            Console.WriteLine("\n      минимальний кредит 500 сом\n");
                            Console.Write("      Введите сумму 500 сом: ");
                        }

                    }
                    var percent = 0;
                    if (creAmout <= 999) percent = 20;
                    else if (creAmout >= 1000 && creAmout <= 5000) percent = 10;
                    else if (creAmout >= 5001 && creAmout <= 20000) percent = 5;
                    else if (creAmout >= 20001) percent = 3;
                    else percent = 0;
                    Console.WriteLine($"      Процентная ставка: {percent}%");
                    Console.Write("\n      Срок кредитования, мес.  : ");
                    var term = 0;
                    while (term <= 0)
                    {
                        term = Convert.ToInt32(Console.ReadLine());
                        if (term <= 0)
                        {
                            Console.WriteLine("\n      минимальний срок кредита 1 мес\n");
                            Console.Write("      Введите срок кредита: ");
                        }
                    }
                    var termS = 0;
                    if (term <= 12) termS = 2;
                    else if (term > 12) termS = 1;
                    else term = 0;
                    Console.WriteLine("\n                   2. Сведения о заявителе");
                    Console.WriteLine($"\n      Имя: {name}");
                    Console.WriteLine($"\n      Фамилия: {lastname}");
                    Console.WriteLine($"\n      Дата рождения: {birhtData}");
                    Console.WriteLine($"\n      Гражданство: {citizen}");
                    Console.WriteLine($"\n      Номер Паспорта: {pasnuber}");
                    Console.Write("\n                   3. Информация о ежемесячных доходах\n");
                    Console.Write("\n      По основоному месту работы: ");
                    var income1 = Convert.ToDecimal(Console.ReadLine());
                    Console.Write("\n      Дополнительный доход: ");
                    var income2 = Convert.ToDecimal(Console.ReadLine());
                    Console.Write($"\n      за год: {(income1 + income2) * 12}\n");

                    var genderS = 0;
                    if (gender == "муж") genderS = 1;
                    else if (gender == "жен") genderS = 2;

                    var statusS = 0;
                    if (status == "холост" || status == "вразводе" || status == "Вразводе" || status == "Холост") statusS = 1;
                    else if (status == "женат" || status == "женат" || status == "вдова" || status == "Вдова" || status == "вдовец" || status == "Вдовец") statusS = 2;
                    else statusS = 0;

                    var bday = Convert.ToDateTime(birhtData);
                    DateTime now = DateTime.Today;
                    int age = now.Year - bday.Year;
                    int birthS = 0;
                    if (age >= 26 && age <= 35) birthS = 1;
                    else if (age >= 36 && age <= 62) birthS = 2;
                    else if (age >= 63) birthS = 1;
                    else birthS = 0;

                    var citizenS = 0;
                    if (citizen == "Таджикистан") citizenS = 1;

                    var incomeS = 0;
                    var incomePersent = creAmout / (income1 + income2) * 100;
                    if (incomePersent <= 80) incomeS = 4;
                    else if (incomePersent >= 81 && incomePersent <= 150) incomeS = 3;
                    else if (incomePersent >= 151 && incomePersent <= 250) incomeS = 2;
                    else if (incomePersent >= 251) incomeS = 1;
                    else incomeS = 0;

                    var creditS = 0;
                    if (creditCount >= 3) creditS = 2;
                    else if (creditCount <= 2) creditS = 1;
                    else if (creditCount == 0) creditS = -1;

                    var delayS = 0;
                    if (delayCount <= 3) delayS = 0;
                    else if (delayCount == 4) delayS = -1;
                    else if (delayCount >= 5 && delayCount <= 7) delayS = -2;
                    else if (delayCount >= 8) delayS = -3;

                    List<int> numbers = new List<int>() { genderS, statusS, birthS, citizenS, incomeS, creditS, delayS, purpS, termS };
                    var sum = 0;
                    int[] array = numbers.ToArray();
                    for (int i = 0; i < array.Length; sum += array[i], i++) ;
                    if (sum <= 11)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Clear();
                        Console.WriteLine("\n\n                         ОТКАЗАНО");
                        Console.ResetColor();

                        var com = conn.CreateCommand();
                        com.CommandText = "INSERT INTO Credit([PersonId], [CreditAmout], [percents], [GetAt], [Term], [Purpouse], [Status], [ActiveNot]) VALUES (@clientId, @creAmout, 0, @date, 0, @purp, 'ОТКАЗАНО', 'оплачено');";
                        com.Parameters.AddWithValue("@clientId", clientId);
                        com.Parameters.AddWithValue("@creAmout", creAmout);
                        com.Parameters.AddWithValue("@date", DateTime.Now);
                        com.Parameters.AddWithValue("@purp", purp);
                        conn.Open();
                        var result = com.ExecuteNonQuery();
                        if (result > 0) Console.WriteLine("                     Кредит неоформлен");
                        conn.Close();
                    }
                    else if (sum > 11)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Clear();
                        Console.WriteLine("\n\n                   Поздравляем вы получили кредит");
                        Console.ResetColor();

                        var com = conn.CreateCommand();
                        com.CommandText = "INSERT INTO Credit([PersonId], [CreditAmout], [percents], [GetAt], [Term], [Purpouse], [Status], [ActiveNot]) VALUES (@clientId, @creAmout, @percent, @date, @term, @purp, 'ОДОБРЕНО', 'активно');";
                        com.Parameters.AddWithValue("@clientId", clientId);
                        com.Parameters.AddWithValue("@creAmout", creAmout);
                        com.Parameters.AddWithValue("@percent", percent);
                        com.Parameters.AddWithValue("@date", DateTime.Now);
                        com.Parameters.AddWithValue("@term", term);
                        com.Parameters.AddWithValue("@purp", purp);
                        conn.Open();
                        var result = com.ExecuteNonQuery();
                        if (result > 0)
                        {
                            Console.WriteLine("                      Кредит успешно оформлен\n");
                            DateTime to = DateTime.Now.Date;
                            to.AddMonths(term);
                            Console.WriteLine($"      Сумма кледита: {creAmout * percent} сом - Срок: {term} мес - График погашение: по {(creAmout * percent) / term} сом в мес - Крайный срок: {to}    ");
                        }
                        conn.Close();
                    }
                }
                connection.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void ClientAccount(string ConString, int clientId, int activecredit)
        {
            try
            {
                var name = "";
                var surname = "";
                var creditCount = 0;
                var i = 1;
                decimal creAmout = 0;
                var getAt = "";
                double term = 0;
                var perouse = "";
                var status = "";
                var actveNot = "";
                decimal paidAmount = 0;
                var payAt = "";
                var dueDate = "";
                var getHistoryCount = 0;
                var getDelayCount = 0;
                var delay = 0;

                var conn = new SqlConnection(ConString);
                conn.Open();
                var command1 = conn.CreateCommand();
                command1.CommandText = $"SELECT * FROM Passport WHERE Id={clientId};";
                var reader1 = command1.ExecuteReader();
                while (reader1.Read())
                {
                    name = !string.IsNullOrEmpty(reader1.GetValue(1)?.ToString()) ? reader1.GetString(1) : null;
                    surname = !string.IsNullOrEmpty(reader1.GetValue(2)?.ToString()) ? reader1.GetString(2) : null;
                }
                reader1.Close();
                var command2 = conn.CreateCommand();
                command2.CommandText = $"SELECT COUNT(PersonId)FROM Credit WHERE PersonId={clientId};";
                var reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    creditCount = reader2.GetInt32(0);
                }
                reader2.Close();

                Console.Write($"                   Личный кабинет: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{surname} {name}\n");
                Console.ResetColor();
                Console.Write("      Общее количество кредитов: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{creditCount}\n");
                Console.ResetColor();

                var command = conn.CreateCommand();
                command.CommandText = $"SELECT * FROM Credit WHERE PersonId={clientId};";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    creAmout = !string.IsNullOrEmpty(reader.GetValue(2)?.ToString()) ? reader.GetDecimal(2) : 0;
                    term = !string.IsNullOrEmpty(reader.GetValue(5)?.ToString()) ? reader.GetInt32(5) : 0;
                    getAt = reader.GetDateTime(4).ToString();
                    perouse = !string.IsNullOrEmpty(reader.GetValue(6)?.ToString()) ? reader.GetString(6) : null;
                    status = !string.IsNullOrEmpty(reader.GetValue(7)?.ToString()) ? reader.GetString(7) : null;
                    actveNot = !string.IsNullOrEmpty(reader.GetValue(8)?.ToString()) ? reader.GetString(8) : null;
                    Console.WriteLine($"      {i}. Сумма:{creAmout}  Цель:{perouse}  Дата заявки:{getAt}  Срок:{term}  статус:{status}\n");
                    i++;

                }

                reader.Close();
                Console.WriteLine("                   Активный кредит\n");
                var b = 1;
                var c = 1;
                var command3 = conn.CreateCommand();
                command3.CommandText = $"SELECT * FROM Credit WHERE PersonId={clientId} AND ActiveNot='активно';";
                var reader3 = command3.ExecuteReader();
                while (reader3.Read())
                {
                    creAmout = !string.IsNullOrEmpty(reader3.GetValue(2)?.ToString()) ? reader3.GetDecimal(2) : 0;
                    term = !string.IsNullOrEmpty(reader3.GetValue(5)?.ToString()) ? reader3.GetInt32(5) : 0;
                    getAt = reader3.GetDateTime(4).ToString();
                    perouse = !string.IsNullOrEmpty(reader3.GetValue(6)?.ToString()) ? reader3.GetString(6) : null;
                    status = !string.IsNullOrEmpty(reader3.GetValue(7)?.ToString()) ? reader3.GetString(7) : null;
                    actveNot = !string.IsNullOrEmpty(reader3.GetValue(8)?.ToString()) ? reader3.GetString(8) : null;
                    Console.WriteLine($"      {b}. Сумма:{creAmout}  Цель:{perouse}  Дата заявки:{getAt}  Срок:{term}  статус:{status}\n");
                    b++;
                }
                reader3.Close();

                var command5 = conn.CreateCommand();
                command5.CommandText = $"SELECT COUNT(UserId)FROM CreditHistory WHERE UserId={clientId} AND CreditId={activecredit};";
                var reader5 = command5.ExecuteReader();
                while (reader5.Read())
                {
                    getHistoryCount = reader5.GetInt32(0);
                }
                reader5.Close();

                var command6 = conn.CreateCommand();
                command6.CommandText = $"SELECT COUNT(delay)FROM CreditHistory WHERE UserId={clientId} AND CreditId={activecredit} AND delay=1;";
                var reader6 = command6.ExecuteReader();
                while (reader6.Read())
                {
                    getDelayCount = reader6.GetInt32(0);
                }
                reader6.Close();

                double onePercent = (term / 100);
                double percent = getHistoryCount / onePercent;
                Console.WriteLine($"      оплачено: {getHistoryCount} мес, {percent}%,    просрочек: {getDelayCount}\n\n");

                Console.WriteLine("                   Все оплаченные месяцы");
                var command4 = conn.CreateCommand();
                command4.CommandText = $"SELECT * FROM CreditHistory WHERE UserId={clientId} AND CreditId={activecredit};";
                var reader4 = command4.ExecuteReader();
                while (reader4.Read())
                {
                    paidAmount = !string.IsNullOrEmpty(reader4.GetValue(1)?.ToString()) ? reader4.GetInt32(1) : 0;
                    delay = !string.IsNullOrEmpty(reader4.GetValue(5)?.ToString()) ? reader4.GetInt32(5) : 0;
                    payAt = reader4.GetDateTime(2).ToString();
                    dueDate = reader4.GetDateTime(3).ToString();
                    var del = "";
                    if (delay == 1) del = "ПРОСРОЧЕНО";
                    else if (delay == 0) del = "НЕ ПРОСРОЧЕНО";
                    Console.WriteLine($"     {c}. Оплачено: {paidAmount}   Дата оплаты: {payAt}   Крайний срок: {dueDate}   Статус: {del}");
                    c++;
                }
                reader.Close();
                conn.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
