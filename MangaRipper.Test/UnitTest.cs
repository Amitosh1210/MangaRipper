﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MangaRipper.Core;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Linq;

namespace MangaRipper.Test
{
    [TestClass]
    public class UnitTest
    {
        CancellationTokenSource source;
        [TestInitialize]
        public void Initialize()
        {
            Framework.Init();
            source = new CancellationTokenSource();
        }

        [TestMethod]
        public async Task MangaReader_Test()
        {
            string url = "http://www.mangareader.net/naruto";
            var service = Framework.GetService(url);
            // Test service can find chapters
            var chapters = await service.FindChapters(url, new Progress<int>(), source.Token);
            Assert.IsTrue(chapters.Count > 0, "Cannot find chapters.");
            // Test chapters are in correct order.
            var lastChapter = chapters.Last();
            Assert.AreEqual("Naruto 1", lastChapter.Name);
            // Test there're no duplicated chapters.
            var anyDuplicated = chapters.GroupBy(x => x.Link).Any(g => g.Count() > 1);
            Assert.IsFalse(anyDuplicated, "There're duplicated chapters.");
            // Test service can find images.
            var firstChapter = chapters.First();
            var images = await service.FindImanges(firstChapter, new Progress<int>(), source.Token);
            Assert.IsTrue(images.Count > 0, "Cannot find images.");
            
        }

        [TestMethod]
        public async Task MangaFox_Test()
        {
            // Test with unlicensed manga. Appveyor CI is US based and cannot access licensed manga in the US. 
            // If we test with a licensed manga, this test will failed.
            string url = "http://mangafox.me/manga/tian_jiang_xian_shu_nan/";
            var service = Framework.GetService(url);
            var chapters = await service.FindChapters(url, new Progress<int>(), source.Token);
            Assert.IsTrue(chapters.Count > 0, "Cannot find chapters.");
            var chapter = chapters[0];
            var images = await service.FindImanges(chapter, new Progress<int>(), source.Token);
            Assert.IsTrue(images.Count > 0, "Cannot find images.");
        }

        [TestMethod]
        public async Task MangaHere_Test()
        {
            string url = "http://www.mangahere.co/manga/the_god_of_high_school/";
            var service = Framework.GetService(url);
            var chapters = await service.FindChapters(url, new Progress<int>(), source.Token);
            Assert.IsTrue(chapters.Count > 0, "Cannot find chapters.");
            var chapter = chapters[0];
            var images = await service.FindImanges(chapter, new Progress<int>(), source.Token);
            Assert.IsTrue(images.Count > 0, "Cannot find images.");
        }

        [TestMethod]
        public async Task MangaShare_Test()
        {
            string url = "http://read.mangashare.com/Beelzebub";
            var service = Framework.GetService(url);
            var chapters = await service.FindChapters(url, new Progress<int>(), source.Token);
            Assert.IsTrue(chapters.Count > 0, "Cannot find chapters.");
            var chapter = chapters[0];
            var images = await service.FindImanges(chapter, new Progress<int>(), source.Token);
            Assert.IsTrue(images.Count > 0, "Cannot find images.");
        }
    }
}
