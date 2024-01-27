
namespace Wild
{
    class Program
    {
        //Определение выходного дня
        static readonly IDictionary<DayOfWeek, int> Weekends = new Dictionary<DayOfWeek, int>
        {
            [DayOfWeek.Saturday] = 2,
            [DayOfWeek.Sunday] = 1
        };
        static void Main(string[] args)
        {
            // Словарь отпусков сотрудников
            var vacationDictionary = new Dictionary<string, List<DateTime>>()
            {
                ["Иванов Иван Иванович"] = new List<DateTime>(),
                ["Петров Петр Петрович"] = new List<DateTime>(),
                ["Юлина Юлия Юлиановна"] = new List<DateTime>(),
                ["Сидоров Сидор Сидорович"] = new List<DateTime>(),
                ["Павлов Павел Павлович"] = new List<DateTime>(),
                ["Георгиев Георг Георгиевич"] = new List<DateTime>()
            };

            // Генерация отпусков для каждого сотрудника
            vacationDictionary = GenerateVacations(vacationDictionary);

            // Вывод отпусков для каждого сотрудника
            foreach (var employee in vacationDictionary)
            {
                Console.WriteLine($"Отпуск для {employee.Key}:");
                foreach (var vacationDate in employee.Value)
                {
                    Console.WriteLine(vacationDate.ToShortDateString());
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Генерация отпусков для входящего списка сотрудников
        /// </summary>
        /// <param name="vacationDictionary"></param>
        /// <returns>Заполненный датами отпусков список сотрудников</returns>
        static Dictionary<string, List<DateTime>> GenerateVacations(Dictionary<string, List<DateTime>> vacationDictionary)
        {
            //Даты отпусков для всех сотрудников
            var vacations = new List<DateTime>();
            var random = new Random();
            var start = new DateTime(DateTime.Today.Year, 1, 1);
            var range = (new DateTime(DateTime.Today.Year + 1, 1, 1) - start).Days;

            // Цикл по сотруднику
            foreach (var employee in vacationDictionary)
            {
                // Список отпусков для текущего сотрудника
                var dateList = employee.Value;
                // Генерация отпусков до достижения общей длительности отпуска в 28 дней
                while (dateList.Count < 28)
                {
                    // Случайным образом определяем дату начала отпуска
                    var startDate = start.AddDays(random.Next(range));

                    // Если начало отпуска попало на выходной, смещаем до понедельника
                    if (Weekends.TryGetValue(startDate.DayOfWeek, out var day))
                        startDate = startDate.AddDays(day);


                    // Выбор случайной продолжительности отпуска: 7 или 14 дней
                    var vacationLength = dateList.Count == 21 
                        ? 7 
                        : random.Next(2) == 0 ? 7 : 14;
                    //Дата окончания текущего отпуска
                    var endDate = startDate.AddDays(vacationLength);

                    // Проверка пересечения отпусков
                    //Проверка что текущий отпуск не пересекается с чужими опусками в следующие и предыдущие 4 дня
                    if (!vacations.Any(element => element >= startDate.AddDays(-3) && element <= endDate.AddDays(3))) 
                    {
                        //Проверка что текущий отпуск не пересекается с другими отпусками текущего сотрудника в ближаший и предыдущий месяца
                        if (!dateList.Any(element => element >= startDate.AddMonths(-1) && element <= endDate.AddMonths(1))) 
                        {
                            //Заполнение текущего отпуска в общий список отпусков и список отпусков текущего сотрудника
                            for (var date = startDate; date < endDate; date = date.AddDays(1)) 
                            {
                                vacations.Add(date);
                                dateList.Add(date);
                            }
                        }
                    }
                }
            }
            return vacationDictionary;
        }
    }
}

