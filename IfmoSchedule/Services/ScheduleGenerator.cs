﻿using System;
using System.Globalization;
using IfmoSchedule.Models;
using IfmoSchedule.Repositories;

namespace IfmoSchedule.Services
{
    public static class ScheduleGenerator
    {
        public static string GenerateMessage(string groupName)
        {
            var msg = GetHeader();
            var date = GenerateNextDay();
            msg += GetScheduleData(date.Day, date.Week, groupName);
            return msg;
        }

        public static string GenerateMessage(string groupName, int day, int week)
        {
            var msg = GetHeader();
            //TODO: check week
            msg += GetScheduleData(day, (Week) week, groupName);
            return msg;
        }

        private static string GetHeader()
        {
            return "Расписание на завтра!\n";
        }

        private static Week GetWeekType(DateTime current)
        {
            var currentWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(current,
                CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
            var resultWeek = (currentWeek - 5) % 2;
            return resultWeek == 0 ? Week.Even : Week.Odd;
        }

        private static (int Day, Week Week) GenerateCurrentDay()
        {
            var current = DateTime.UtcNow;
            var todayWeek = GetWeekType(current);
            return ((int) current.DayOfWeek - 1, todayWeek);
        }

        private static (int Day, Week Week) GenerateNextDay()
        {
            var current = DateTime.UtcNow.AddDays(1);
            var todayWeek = GetWeekType(current);
            return ((int) current.DayOfWeek - 1, todayWeek);
        }

        private static string GetScheduleData(int day, Week weekType, string groupName)
        {
            var answer = "";
            var my = new LessonStorageRepository(groupName);
            var lessonList = my.GetLesson(day, weekType);

            foreach (var item in lessonList)
            {
                var room = item.Title == "Иностранный язык" ? "" : $"ауд. {item.Room} ";
                answer += $"{item.TimeBegin} -> {item.Title} ({item.Status}, {room}{item.Place})\n";
            }

            return answer;
        }
    }
}