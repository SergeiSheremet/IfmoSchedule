﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using IfmoSchedule.Models;
using Newtonsoft.Json;

namespace IfmoSchedule.Repositories
{
    public class LocalStorageRepository
    {
        private static readonly string FileName = @"lastFriendsStats.json";

        public static List<LessonModel> CreateFromFile()
        {
            return File.Exists(FileName) ? JsonConvert.DeserializeObject<List<LessonModel>>(File.ReadAllText(FileName)) : null;
        }

        public IEnumerable<LessonModel> GetLesson(int day, Week weekType)
        {
            return CreateFromFile()
                .Where(i =>
                    i.DayOfWeek == day && i.WeekType == (int)weekType
                    || i.DayOfWeek == day && i.WeekType == 0)
                .Where(i => i.Title != "Физическая культура (элективная дисциплина)");
        }
    }
}