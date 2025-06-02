using landlord_be.Models;
using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0090
namespace landlord_be.Data
{
    public class DbInitializer
    {
        private readonly ILogger _logger;

        public DbInitializer(ILogger logger)
        {
            _logger = logger;
        }

        public async Task Initialize(ApplicationDbContext context)
        {
            var rd = new Random();

            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            DateTime time = DateTime.Now.ToUniversalTime();

            var personals = new List<Personal>
            {
                new Personal
                {
                    FirstName = "Иван",
                    LastName = "Иванов",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Петр",
                    LastName = "Петров",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Сергей",
                    LastName = "Сергеев",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Алексей",
                    LastName = "Алексеев",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Дмитрий",
                    LastName = "Дмитриев",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Анна",
                    LastName = "Антонова",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Мария",
                    LastName = "Маркова",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Елена",
                    LastName = "Еленина",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Олег",
                    LastName = "Олегов",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Т1атьяна",
                    LastName = "Татьянова",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Т2атьяна",
                    LastName = "Татьянова",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Т3атьяна",
                    LastName = "Татьянова",
                    Patronym = "Иванович",
                },
            };

            personals.ForEach(p => context.Personals.Add(p));
            await context.SaveChangesAsync();

            var users = new List<User>();

            for (int i = 0; i < 11; i++)
            {
                users.Add(
                    new User
                    {
                        PersonalId = personals[i].Id,
                        NumberHash = "hash1",
                        Email = "some@mail.ru",
                        Token = "token1",
                        RegisterDate = time,
                        UpdateDate = time,
                    }
                );
            }

            users.ForEach(u => context.Users.Add(u));
            await context.SaveChangesAsync();

            var addresses = new List<Address>
            {
                new Address
                {
                    City = "Санкт-Петербург",
                    Region = "Санкт-Петербург",
                    Street = "Невский проспект",
                    House = "28",
                    Floor = 3,
                },
                new Address
                {
                    City = "Новосибирск",
                    Region = "Новосибирская обл",
                    Street = "Красный проспект",
                    House = "12",
                    Floor = 4,
                },
                new Address
                {
                    City = "Екатеринбург",
                    Region = "Свердловская обл",
                    Street = "Ленина",
                    House = "35",
                    Floor = 2,
                },
                new Address
                {
                    City = "Казань",
                    Region = "Татарстан",
                    Street = "Баумана",
                    House = "45",
                    Floor = 6,
                },
                new Address
                {
                    City = "Нижний Новгород",
                    Region = "Нижегородская обл",
                    Street = "Большая Покровская",
                    House = "10",
                    Floor = 1,
                },
                new Address
                {
                    City = "Челябинск",
                    Region = "Челябинская обл",
                    Street = "Кирова",
                    House = "22",
                    Floor = 5,
                },
                new Address
                {
                    City = "Омск",
                    Region = "Омская обл",
                    Street = "Ленина",
                    House = "15",
                    Floor = 3,
                },
                new Address
                {
                    City = "Ростов-на-Дону",
                    Region = "Ростовская обл",
                    Street = "Садовая",
                    House = "50",
                    Floor = 7,
                },
                new Address
                {
                    City = "Уфа",
                    Region = "Башкортостан",
                    Street = "Ленина",
                    House = "5",
                    Floor = 2,
                },
                new Address
                {
                    City = "Волгоград",
                    Region = "Волгоградская обл",
                    Street = "Мира",
                    House = "18",
                    Floor = 4,
                },
                new Address
                {
                    City = "Пермь",
                    Region = "Пермский край",
                    Street = "Сибирская",
                    House = "30",
                    Floor = 6,
                },
                new Address
                {
                    City = "Тюмень",
                    Region = "Тюменская обл",
                    Street = "Московская",
                    House = "8",
                    Floor = 1,
                },
                new Address
                {
                    City = "Ижевск",
                    Region = "Удмуртия",
                    Street = "Кирова",
                    House = "12",
                    Floor = 3,
                },
                new Address
                {
                    City = "Барнаул",
                    Region = "Алтайский край",
                    Street = "Ленина",
                    House = "25",
                    Floor = 5,
                },
            };

            addresses.ForEach(a => context.Addresses.Add(a));
            await context.SaveChangesAsync();

            var properties = new List<Property>
            {
                new Property
                {
                    OwnerId = users[0].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Уютная квартира",
                    Desc = "Квартира в центре города с современным ремонтом.",
                    AddressId = addresses[0].Id,
                    Area = 75,
                    Rooms = 2,
                    Services = true,
                    Parking = false,
                    Price = 50000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.5f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[1].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Семейный дом",
                    Desc = "Просторный дом с садом и гаражом.",
                    AddressId = addresses[1].Id,
                    Area = 200,
                    Rooms = 5,
                    Services = true,
                    Parking = true,
                    Price = 12000000,
                    Currency = 125,
                    Rating = 4.8f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[2].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Студия",
                    Desc = "Современная студия в новом доме.",
                    AddressId = addresses[2].Id,
                    Area = 40,
                    Rooms = 1,
                    Services = false,
                    Parking = false,
                    Price = 30000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.0f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[3].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Квартира с балконом",
                    Desc = "Квартира с балконом и видом на парк.",
                    AddressId = addresses[3].Id,
                    Area = 85,
                    Rooms = 3,
                    Services = true,
                    Parking = true,
                    Price = 8000000,
                    Currency = 125,
                    Rating = 4.6f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[4].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Дача",
                    Desc = "Уютная дача на природе.",
                    AddressId = addresses[4].Id,
                    Area = 100,
                    Rooms = 4,
                    Services = true,
                    Parking = true,
                    Price = 20000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.2f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[5].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Люкс квартира",
                    Desc = "Элитная квартира в центре города.",
                    AddressId = addresses[5].Id,
                    Area = 120,
                    Rooms = 3,
                    Services = true,
                    Parking = true,
                    Price = 15000000,
                    Currency = 125,
                    Rating = 5.0f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[6].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Квартира-студия",
                    Desc = "Студия с современным дизайном.",
                    AddressId = addresses[6].Id,
                    Area = 50,
                    Rooms = 1,
                    Services = false,
                    Parking = false,
                    Price = 35000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.3f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[7].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Коттедж",
                    Desc = "Коттедж с большим участком.",
                    AddressId = addresses[7].Id,
                    Area = 250,
                    Rooms = 6,
                    Services = true,
                    Parking = true,
                    Price = 20000000,
                    Currency = 125,
                    Rating = 4.9f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[8].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Комфортная квартира",
                    Desc = "Квартира с хорошей планировкой и ремонтом.",
                    AddressId = addresses[8].Id,
                    Area = 70,
                    Rooms = 2,
                    Services = true,
                    Parking = true,
                    Price = 45000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.4f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[9].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Квартира с террасой",
                    Desc = "Квартира с террасой и видом на реку.",
                    AddressId = addresses[9].Id,
                    Area = 95,
                    Rooms = 3,
                    Services = true,
                    Parking = true,
                    Price = 9500000,
                    Currency = 125,
                    Rating = 4.7f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[0].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Современный дом",
                    Desc = "Дом с современными удобствами и большим двором.",
                    AddressId = addresses[10].Id,
                    Area = 180,
                    Rooms = 4,
                    Services = true,
                    Parking = true,
                    Price = 60000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.6f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[1].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Квартира в новостройке",
                    Desc = "Новая квартира с ремонтом в современном доме.",
                    AddressId = addresses[11].Id,
                    Area = 80,
                    Rooms = 3,
                    Services = true,
                    Parking = true,
                    Price = 8500000,
                    Currency = 125,
                    Rating = 4.8f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[2].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Квартира с видом на город",
                    Desc = "Квартира с панорамными окнами и видом на город.",
                    AddressId = addresses[12].Id,
                    Area = 90,
                    Rooms = 3,
                    Services = true,
                    Parking = true,
                    Price = 70000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.5f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[3].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Деревянный дом",
                    Desc = "Экологически чистый деревянный дом в лесу.",
                    AddressId = addresses[13].Id,
                    Area = 150,
                    Rooms = 5,
                    Services = true,
                    Parking = true,
                    Price = 11000000,
                    Currency = 125,
                    Rating = 4.9f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[0].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Commercial,
                    Name = "Офис в центре города",
                    Desc = "Современный офис с открытой планировкой и всеми удобствами.",
                    AddressId = addresses[0].Id,
                    Area = 120,
                    Rooms = 1,
                    Services = true,
                    Parking = true,
                    Price = 80000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.5f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[1].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Commercial,
                    Name = "Торговое помещение",
                    Desc = "Помещение для магазина в проходимом месте.",
                    AddressId = addresses[1].Id,
                    Area = 150,
                    Rooms = 1,
                    Services = true,
                    Parking = true,
                    Price = 15000000,
                    Currency = 125,
                    Rating = 4.7f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[2].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Commercial,
                    Name = "Коворкинг",
                    Desc = "Просторный коворкинг с удобными рабочими местами.",
                    AddressId = addresses[2].Id,
                    Area = 200,
                    Rooms = 1,
                    Services = true,
                    Parking = true,
                    Price = 60000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.6f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[3].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Commercial,
                    Name = "Складское помещение",
                    Desc = "Склад с удобным доступом и охраной.",
                    AddressId = addresses[3].Id,
                    Area = 300,
                    Rooms = 0,
                    Services = true,
                    Parking = true,
                    Price = 5000000,
                    Currency = 125,
                    Rating = 4.4f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[4].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Commercial,
                    Name = "Ресторан",
                    Desc = "Ресторан с полностью оборудованной кухней и залом.",
                    AddressId = addresses[4].Id,
                    Area = 250,
                    Rooms = 2,
                    Services = true,
                    Parking = true,
                    Price = 120000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.8f,
                    Status = PropertyStatus.Active,
                },
            };

            properties.ForEach(p => context.Properties.Add(p));
            await context.SaveChangesAsync();

            var savedProperties = await context.Properties.ToListAsync();

            // Define attribute pool with 15 unique attribute types
            var attributePool = new List<(
                string Name,
                PropertyAttributeType Type,
                List<string> PossibleValues
            )>
            {
                (
                    "Залог",
                    PropertyAttributeType.Number,
                    new List<string> { "50000", "100000", "150000", "200000", "0" }
                ),
                (
                    "Оплата ЖКХ",
                    PropertyAttributeType.Text,
                    new List<string> { "Включено", "Не включено", "Частично включено" }
                ),
                (
                    "Срок аренды",
                    PropertyAttributeType.Text,
                    new List<string> { "от 1 месяца", "от 3 месяцев", "от 6 месяцев", "от 1 года" }
                ),
                (
                    "Домашние животные",
                    PropertyAttributeType.Text,
                    new List<string> { "Разрешены", "Не разрешены", "По договоренности" }
                ),
                (
                    "Интернет",
                    PropertyAttributeType.Text,
                    new List<string> { "Оптоволокно", "ADSL", "Wi-Fi", "Не подключен" }
                ),
                (
                    "Отопление",
                    PropertyAttributeType.Text,
                    new List<string> { "Центральное", "Автономное", "Электрическое", "Газовое" }
                ),
                (
                    "Этаж",
                    PropertyAttributeType.Number,
                    new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }
                ),
                (
                    "Мебель",
                    PropertyAttributeType.Text,
                    new List<string>
                    {
                        "Полностью меблирована",
                        "Частично меблирована",
                        "Без мебели",
                    }
                ),
                (
                    "Техника",
                    PropertyAttributeType.Text,
                    new List<string> { "Полный набор", "Частично", "Без техники" }
                ),
                (
                    "Ремонт",
                    PropertyAttributeType.Text,
                    new List<string>
                    {
                        "Евроремонт",
                        "Косметический",
                        "Требует ремонта",
                        "Дизайнерский",
                    }
                ),
                (
                    "Парковка",
                    PropertyAttributeType.Text,
                    new List<string> { "Гараж", "Парковочное место", "Во дворе", "Платная", "Нет" }
                ),
                ("Балкон", PropertyAttributeType.Boolean, new List<string> { "true", "false" }),
                ("Лифт", PropertyAttributeType.Boolean, new List<string> { "true", "false" }),
                ("Охрана", PropertyAttributeType.Boolean, new List<string> { "true", "false" }),
                (
                    "Кондиционер",
                    PropertyAttributeType.Boolean,
                    new List<string> { "true", "false" }
                ),
            };

            var propertyAttributes = new List<PropertyAttribute>();
            var random = new Random();

            // Generate attributes for each property
            for (int i = 0; i < savedProperties.Count; i++)
            {
                // Randomly select 5-9 attributes for each property
                var attributeCount = random.Next(5, 10);
                var selectedAttributes = attributePool
                    .OrderBy(x => random.Next())
                    .Take(attributeCount);

                foreach (var (name, type, possibleValues) in selectedAttributes)
                {
                    var value = possibleValues[random.Next(possibleValues.Count)];

                    propertyAttributes.Add(
                        new PropertyAttribute
                        {
                            PropertyId = savedProperties[i].Id,
                            Name = name,
                            Value = value,
                            AttributeType = type,
                            IsSearchable = true,
                        }
                    );
                }
            }

            propertyAttributes.ForEach(a => context.Attributes.Add(a));
            await context.SaveChangesAsync();

            var imageLinks = new List<ImageLink>();

            for (int i = 0; i < savedProperties.Count; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    imageLinks.Add(
                        new ImageLink
                        {
                            PropertyId = savedProperties[i].Id,
                            Link = "public/images/" + ((i + 1) % 8) + "_" + (j + 1) + ".webp",
                        }
                    );
                }
            }

            imageLinks.ForEach(i => context.ImageLinks.Add(i));
            await context.SaveChangesAsync();

            // Add this after the imageLinks.ForEach(i => context.ImageLinks.Add(i)); line and before context.SaveChanges();

            var calendarEntryPool = new List<(
                string Name,
                string Description,
                CalendarState State,
                int DurationDays
            )>
            {
                (
                    "Ремонт",
                    "Проведен ремонт в квартире, были переклеены обои на кухне и заменена сантехника в ванной комнате.",
                    CalendarState.Maintenance,
                    7
                ),
                (
                    "Генеральная уборка",
                    "Выполнена генеральная уборка после предыдущих жильцов. Помыты все окна, почищены ковры, обработаны все поверхности дезинфицирующими средствами.",
                    CalendarState.Maintenance,
                    2
                ),
                (
                    "Аренда семье Петровых",
                    "Квартира сдана в аренду молодой семье с ребенком. Договор подписан на год с возможностью продления. Залог внесен в полном объеме.",
                    CalendarState.Rented,
                    30
                ),
                (
                    "Косметический ремонт",
                    "Проведена покраска стен в гостиной и спальне, заменены розетки и выключатели. Установлены новые плинтуса по всей квартире.",
                    CalendarState.Maintenance,
                    5
                ),
                (
                    "Долгосрочная аренда",
                    "Квартира сдана студенту на время обучения в университете. Договор заключен на 2 года. Арендатор очень ответственный и аккуратный.",
                    CalendarState.Rented,
                    60
                ),
                (
                    "Замена сантехники",
                    "Полная замена сантехники в ванной комнате и туалете. Установлена новая ванна, унитаз, раковина и смесители. Работы выполнены качественно.",
                    CalendarState.Maintenance,
                    4
                ),
                (
                    "Краткосрочная аренда",
                    "Квартира сдается на месяц командированному специалисту. Все коммунальные услуги включены в стоимость аренды.",
                    CalendarState.Rented,
                    30
                ),
                (
                    "Замена окон",
                    "Установлены новые пластиковые окна во всех комнатах. Значительно улучшилась тепло- и звукоизоляция квартиры.",
                    CalendarState.Maintenance,
                    3
                ),
                (
                    "Подготовка к продаже",
                    "Квартира готовится к продаже. Проведен косметический ремонт, обновлена мебель, сделана профессиональная фотосъемка для объявления.",
                    CalendarState.Blocked,
                    14
                ),
                (
                    "Аренда молодой паре",
                    "Сдано молодой паре без детей и домашних животных. Очень чистоплотные арендаторы, всегда вовремя платят за аренду.",
                    CalendarState.Rented,
                    90
                ),
                (
                    "Ремонт электрики",
                    "Полная замена электропроводки в квартире. Установлены новые автоматы в щитке, добавлены дополнительные розетки в комнатах.",
                    CalendarState.Maintenance,
                    6
                ),
                (
                    "Бронирование",
                    "Квартира забронирована для туристов на новогодние праздники. Предоплата получена, ожидается заселение через неделю.",
                    CalendarState.Reserved,
                    10
                ),
                (
                    "Ремонт пола",
                    "Замена напольного покрытия во всех комнатах. Уложен новый ламинат, установлены пороги. Квартира выглядит как новая.",
                    CalendarState.Maintenance,
                    8
                ),
                (
                    "Семейная аренда",
                    "Квартира сдана семье с двумя детьми школьного возраста. Родители работают в соседнем офисном центре, очень удобное расположение для них.",
                    CalendarState.Rented,
                    180
                ),
                (
                    "Техническое обслуживание",
                    "Проведено плановое техническое обслуживание: проверка всех систем, чистка вентиляции, профилактика бытовой техники.",
                    CalendarState.Maintenance,
                    1
                ),
            };

            var calendarPeriods = new List<CalendarPeriod>();
            var calendarRandom = new Random();

            // Generate calendar entries for first 10 properties
            for (int i = 0; i < savedProperties.Count; i++)
            {
                var currentDate = DateTime.UtcNow.AddDays(-90); // Start from 3 months ago
                var propertyId = savedProperties[i].Id;
                var ownerId = savedProperties[i].OwnerId;

                // Generate 2-4 calendar entries per property
                var entryCount = calendarRandom.Next(2, 5);

                for (int j = 0; j < entryCount; j++)
                {
                    var entry = calendarEntryPool[calendarRandom.Next(calendarEntryPool.Count)];
                    var startDate = currentDate.AddDays(calendarRandom.Next(0, 30));
                    var endDate = startDate.AddDays(
                        entry.DurationDays + calendarRandom.Next(-2, 5)
                    );

                    // For rented periods, sometimes attach a user
                    int? attachedUserId = null;
                    if (entry.State == CalendarState.Rented && calendarRandom.Next(0, 2) == 0)
                    {
                        // Attach a random user that's not the owner
                        var availableUsers = users.Where(u => u.Id != ownerId).ToList();
                        if (availableUsers.Any())
                        {
                            attachedUserId = availableUsers[
                                calendarRandom.Next(availableUsers.Count)
                            ].Id;
                        }
                    }

                    calendarPeriods.Add(
                        new CalendarPeriod
                        {
                            PropertyId = propertyId,
                            StartDate = startDate,
                            EndDate = endDate,
                            State = entry.State,
                            Name = entry.Name,
                            Description = entry.Description,
                            AttachedUserId = attachedUserId,
                            CreatedAt = DateTime.UtcNow.AddDays(-calendarRandom.Next(1, 30)),
                            UpdatedAt = DateTime.UtcNow.AddDays(-calendarRandom.Next(0, 10)),
                        }
                    );

                    // Move current date forward to avoid overlaps
                    currentDate = endDate.AddDays(calendarRandom.Next(1, 15));
                }
            }

            calendarPeriods.ForEach(cp => context.CalendarPeriods.Add(cp));
            await context.SaveChangesAsync();
        }
    }
}
