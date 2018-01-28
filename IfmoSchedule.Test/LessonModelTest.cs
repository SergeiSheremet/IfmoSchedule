﻿using System.Linq;
using IfmoSchedule.Repositories;
using IfmoSchedule.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IfmoSchedule.Test
{
    [TestClass]
    public class LessonModelTest
    {
        [TestMethod]
        public void ModelPropertyTest()
        {
            var repository = new LessonStorageRepository("M3205");
            var list = repository.GetAllLesson();
            Assert.IsNotNull(list);
            var data = list.First();

            Assert.IsNotNull(data.DayOfWeek);
            Assert.IsNotNull(data.Place);
            Assert.IsNotNull(data.Room);
            Assert.IsNotNull(data.Status);
            //Assert.IsNotNull(data.Teacher);
            Assert.IsNotNull(data.TimeBegin);
            Assert.IsNotNull(data.Title);
            Assert.IsNotNull(data.WeekType);
        }
    }
}