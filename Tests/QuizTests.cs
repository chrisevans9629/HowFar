﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using HowFar.Core.Models;
using HowFarApp.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;

namespace HowFar.Tests
{
    [TestFixture]
    public class QuizTests
    {
        private QuizGenerator quiz;
        private Fixture fixture;
        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("test").Options;
            fixture.Inject<Func<DatabaseContext>>(() => new DatabaseContext(options));
            fixture.Inject(fixture.Build<ObjectRepository>().OmitAutoProperties().Create() as IObjectRepository);
            fixture.Inject(fixture.Build<MeasureConverters>().OmitAutoProperties().Create() as IMeasureConverters);
            quiz = fixture.Create<QuizGenerator>();
        }

        [Test, AutoData]
        [Repeat(10)]
        public void CombinatorTest(List<string> data)
        {
            var random = data.Combinator();

            // Assert.True(random.All(p=>p.GroupBy(r=>r).Count() == 1));
            Assert.AreEqual(data.Count, random.First().Count());
            Assert.AreEqual(data.Count * 2, random.Count());
        }


        [Test, AutoData]
        [Repeat(10)]
        public void RandomTest(List<string> data, int seed)
        {
            var random = data.Randomize(new Random(seed));

            Assert.AreEqual(data.Count, random.Count());
            Assert.AreEqual(data.GroupBy(p => p).Count(), random.GroupBy(p => p).Count());
        }

        [Test]
        public void CorrectAnswerMatchesConverter()
        {
            var quizResult = quiz.CreateQuiz(20);
            var converter = fixture.Create<IMeasureConverters>();
            foreach (var quizResultAnswer in quizResult.Answers)
            {
                Assert.AreEqual(converter.Convert(quizResultAnswer.Question.From, quizResultAnswer.Question.To, quizResultAnswer.Question.FromQuantity), quizResultAnswer.CorrectAnswer);
            }
        }

        [Test]
        public void NoQuestionIsTheSame([Random(1, 50, 20)]int size)
        {
            var quizResult = quiz.CreateQuiz(size);
            Assert.True(quizResult.Questions.GroupBy(p => new { p.From.SingleName, To = p.To.SingleName, p.FromQuantity }).All(r => r.Count() == 1));
        }


        [Test]
        public void QuizGreaterThan1()
        {
            var result = quiz.CreateQuiz(10, Core.Models.QuizDifficulty.Beginner);

            foreach (var resultAnswer in result.Answers)
            {
                Assert.True(resultAnswer.CorrectAnswer > 1);
            }
        }

        [Test]
        public void QuizDifficulty()
        {
            var result = quiz.CreateQuiz(10, Core.Models.QuizDifficulty.Beginner);

            foreach (var resultAnswer in result.Answers)
            {
                var value = resultAnswer.CorrectAnswer;
                if (value < 0)
                {
                    value = 1 / value;
                }
                Assert.True(value <= (int)Core.Models.QuizDifficulty.Beginner);
            }
        }

        [Test]
        public void NoQuestionIsTheSameQuestion()
        {
            var quizResult = quiz.CreateQuiz(10);
            Assert.True(quizResult.Questions.GroupBy(p => new { p.From.SingleName, To = p.To.SingleName }).All(r => r.Count() == 1));
        }

        [Test, AutoData]
        public void FromToNotTheSame()
        {
            var quizResult = quiz.CreateQuiz(50);

            foreach (var quizResultQuestion in quizResult.Questions)
            {
                Assert.AreNotEqual(quizResultQuestion.From, quizResultQuestion.To);
            }
        }

        [Test, AutoData]
        public void QuizTextUsesCorrectGrammer()
        {
            var quizResult = quiz.CreateQuiz(10);

            foreach (var quizResultQuestion in quizResult.Questions)
            {
                Assert.AreEqual($"How many '{quizResultQuestion.To.PluralName}' are in {quizResultQuestion.FromQuantity} '{quizResultQuestion.From.PluralName}'?", quizResultQuestion.QuestionText);
            }

        }

        [Test, AutoData]
        public void QuizGeneratorTest()
        {
            var quizResult = quiz.CreateQuiz(20);

            Assert.AreEqual(20, quizResult.Questions.Count);
        }
    }
}