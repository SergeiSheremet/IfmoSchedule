﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IfmoSchedule.Models;
using IfmoSchedule.Repositories;
using IfmoSchedule.Tools;

namespace IfmoSchedule.Services
{
    public static class ScheduleGenerator
    {
        public static string GenerateMessage(string groupName)
        {
            var date = GenerateNextDay();
            var msg = TextConverter.GenerateHeader(date.Week, date.Day);
            return msg + GetScheduleData(groupName, date.Week, date.Day);
        }

        public static string GenerateMessage(string groupName, int week, int day)
        {
            var msg = TextConverter.GenerateHeader((Week)week, day);
            msg += GetScheduleData(groupName, (Week)week, day);
            return msg;
        }

        private static Week GetWeekType(DateTime currentTime)
        {
            var currentWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                time: currentTime,
                rule: CalendarWeekRule.FirstFourDayWeek,
                firstDayOfWeek: DayOfWeek.Monday);
            //TODO: DANGER ZONE
            var resultWeek = (currentWeek - 5) % 2;
            return resultWeek == 0 ? Week.Even : Week.Odd;
        }

        private static (int Day, Week Week) GenerateCurrentDay()
        {
            var current = DateTime.UtcNow;
            var todayWeek = GetWeekType(current);
            return ((int)current.DayOfWeek - 1, todayWeek);
        }

        private static (int Day, Week Week) GenerateNextDay()
        {
            var current = DateTime.UtcNow.AddDays(1);
            var todayWeek = GetWeekType(current);
            return ((int)current.DayOfWeek - 1, todayWeek);
        }

        private static string GetScheduleData(string groupName, Week weekType, int day)
        {
            var answer = "";
            var my = new ServerStorageRepository(groupName);
            var lessonList = my.GetLesson(day, weekType).ToList();

            if (!lessonList.Any())
            {
                //TODO: move to TextConvertor
                answer = "🔮 Пар не будет, ура!";
                return answer;
            }

            foreach (var item in lessonList)
            {
                answer += item.ToString();
                //var room = item.Title == "Иностранный язык" ? "" : $"ауд. {item.Room} ";
                //answer += $"📌 {item.TimeBegin} -> {item.Title} ({item.Status}), {room}{item.Place}\n";
            }
            return answer;
        }

        public static bool CompareData(List<LessonModel> local, List<LessonModel> serverResponse)
        {
            if (local.Count != serverResponse.Count) return false;
            var isSame = true;
            for (var i = 0; i < local.Count; i++)
            {
                isSame = isSame && local[i].Equals(serverResponse[i]);
            }

            return isSame;
        }
    }
}