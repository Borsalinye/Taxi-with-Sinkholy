using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Taxi.Models.Persons.Workers;

namespace Taxi.Models
{
    public static class Generator
    {
        private static readonly string[] OperatorCodes = new string[]
        {
            "910", "915", "916",
            "917", "919", "985",
            "986", "903", "905",
            "906", "909", "962",
            "963", "964", "965",
            "966", "967", "968",
            "969", "980", "983",
            "986", "925", "926",
            "929", "936", "999",
            "901", "958", "977",
            "995", "996"
        };
        private static readonly char[] GovermentNumberLetters = new char[]
        {
            'А', 'В', 'Е',
            'К', 'М', 'Н',
            'О', 'Р', 'С',
            'Т', 'У', 'Х'
        };
        private static readonly Random r = new Random();

        private static readonly Dictionary<int, string> SubjectRF = new Dictionary<int, string>
        {
            { 45, "Москве" },
            { 46 ,"Московской области" }
        };

        private static readonly Dictionary<string, bool> Name = new Dictionary<string, bool>
        {
            { "Андрей", true },     { "Галина", false },
            { "Леонид", true },     { "Кристина", false },
            { "Захар", true },      { "Полина", false },
            { "Олег", true },       { "Дарья", false },
            { "Петр", true },       { "Оксана", false },
            { "Владимир", true },   { "Ульяна", false },
            { "Георгий", true },    { "Алла", false },
            { "Тимофей", true },    { "Жанна", false },
            { "Иван", true },       { "Ирина", false },
            { "Вадим", true },      { "Марина", false },
        };

        private static readonly string[] SurName = new string[]
        {
            "Епифанцев",    "Фомин",
            "Панов",        "Колесников",
            "Сысоев",       "Бобров",
            "Чернов",       "Кабанов",
            "Чернов",       "Ершов",
            "Комиссаров",   "Савельев",
            "Крылов",       "Мухин",
            "Дьячков",      "Баранов",
            "Архипов",      "Крюков",
            "Лебедев",      "Лукин",
        };

        private static readonly string[] MidName = new string[]
        {
            "Андреев",       "Кириллов",
            "Леонидов",      "Александров",
            "Захаров",       "Арсеньев",
            "Олегов",        "Евгеньв",
            "Петров",        "Борисов",
            "Владимиров",    "Эдуардов",
            "Георгиев",      "Вячеславов",
            "Тимофеев",      "Платонов",
            "Иванов",        "Харитонов",
            "Вадимов",       "Юрьев",
        };

        public static string RandomPhone()
        {
            StringBuilder phone = new StringBuilder();
            phone.Append(7);
            phone.Append(OperatorCodes[r.Next(0, OperatorCodes.Length)]);
            for (int i = 0; i < 3; i++)
            {
                phone.Append(r.Next(0, 9));
            }
            for (int i = 0; i < 2; i++)
            {
                phone.Append(r.Next(0, 9));
            }
            for (int i = 0; i < 2; i++)
            {
                phone.Append(r.Next(0, 9));
            }
            phone.Insert(0, "+");
            phone.Insert(2, "(");
            phone.Insert(6, ")");
            phone.Insert(10, "-");
            phone.Insert(13, "-");
            return phone.ToString();
        }

        public static string RandomGovNum()
        { 
            StringBuilder GovNum = new StringBuilder();
            GovNum.Append(GovermentNumberLetters[r.Next(0, GovermentNumberLetters.Length)]);
            for(int i = 0; i < 3; i++)
            {
                GovNum.Append(r.Next(0, 9));
            }
            for(int i = 0; i < 2; i++)
            {
                GovNum.Append(GovermentNumberLetters[r.Next(0, GovermentNumberLetters.Length)]);
            }
            return GovNum.ToString();
        }
        public static string RandomPassport(Worker worker)
        {
            StringBuilder passport = new StringBuilder();
            int SubjRF = SubjectRF.Keys.Where(v => v == worker.SubjRF).FirstOrDefault();
            passport.Insert(0, SubjRF.ToString(), 1);
            passport.Insert(2, worker.BirthDate.ToString(), 1);
            passport.Insert(4, " ", 1);
            for (int i = 0; i < 6; i++)
            {
                passport.Append(r.Next(0, 9));
            }
            passport.Insert(11, $" Выдан отделением УФМС России по {SubjectRF[SubjRF]}", 1);
            return passport.ToString();
        }
        public static string RandomInn(Worker worker)
        {
            StringBuilder Inn = new StringBuilder();
            Inn.Insert(0, SubjectRF.Keys.Where(v => v == worker.SubjRF).FirstOrDefault().ToString(), 1);
            for (int i = 0; i < 10; i++)
            {
                Inn.Append(r.Next(0, 9));
            }
            return Inn.ToString();
        }
        public static string RandomBirthFIO()
        {
            StringBuilder BFIO = new StringBuilder();
            string name = Name.Keys.ElementAt(r.Next(0, Name.Keys.Count));
            string surname = SurName[r.Next(0, SurName.Length)];
            string midname = MidName[r.Next(0, MidName.Length)];
            string birthdate = r.Next(1945, DateTime.Now.Year - 18).ToString();
            BFIO.Insert(0, name);
            switch (Name[name])
            {
                case false:
                    surname += "а";
                    midname += "на";
                    break;
                case true:
                    midname += "ич";
                    break;
            }
            BFIO.Append("\u0020" + surname);
            BFIO.Append("\u0020" + midname);
            BFIO.Append("\u0020" + birthdate);
            return BFIO.ToString();
        }
    }
}