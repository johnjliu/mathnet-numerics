﻿// <copyright file="LogNormalTests.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
// Copyright (c) 2009-2010 Math.NET
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using System.Linq;
using MathNet.Numerics.Distributions;
using NUnit.Framework;

namespace MathNet.Numerics.UnitTests.DistributionTests.Continuous
{
    using Random = System.Random;

    /// <summary>
    /// <c>LogNormal</c> distribution tests.
    /// </summary>
    [TestFixture]
    public class LogNormalTests
    {
        /// <summary>
        /// Set-up parameters.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Control.CheckDistributionParameters = true;
        }

        /// <summary>
        /// Can create <c>LogNormal</c>.
        /// </summary>
        /// <param name="mu">Mu parameter.</param>
        /// <param name="sigma">Sigma value.</param>
        [TestCase(0.0, 0.0)]
        [TestCase(10.0, 0.1)]
        [TestCase(-5.0, 1.0)]
        [TestCase(0.0, 10.0)]
        [TestCase(10.0, 100.0)]
        [TestCase(-5.0, Double.PositiveInfinity)]
        public void CanCreateLogNormal(double mu, double sigma)
        {
            var n = new LogNormal(mu, sigma);
            Assert.AreEqual(mu, n.Mu);
            Assert.AreEqual(sigma, n.Sigma);
        }

        /// <summary>
        /// <c>LogNormal</c> create fails with bad parameters.
        /// </summary>
        /// <param name="mu">Mu parameter.</param>
        /// <param name="sigma">Sigma value.</param>
        [TestCase(Double.NaN, 1.0)]
        [TestCase(1.0, Double.NaN)]
        [TestCase(Double.NaN, Double.NaN)]
        [TestCase(1.0, -1.0)]
        public void LogNormalCreateFailsWithBadParameters(double mu, double sigma)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LogNormal(mu, sigma));
        }

        /// <summary>
        /// Validate ToString.
        /// </summary>
        [Test]
        public void ValidateToString()
        {
            var n = new LogNormal(1d, 2d);
            Assert.AreEqual("LogNormal(μ = 1, σ = 2)", n.ToString());
        }

        /// <summary>
        /// Can set sigma.
        /// </summary>
        /// <param name="sigma">Sigma value.</param>
        [TestCase(-0.0)]
        [TestCase(0.0)]
        [TestCase(0.1)]
        [TestCase(1.0)]
        [TestCase(10.0)]
        [TestCase(Double.PositiveInfinity)]
        public void CanSetSigma(double sigma)
        {
            new LogNormal(1.0, 2.0)
            {
                Sigma = sigma
            };
        }

        /// <summary>
        /// Set sigma fails with negative value.
        /// </summary>
        [Test]
        public void SetSigmaFailsWithNegativeSigma()
        {
            var n = new LogNormal(1.0, 2.0);
            Assert.Throws<ArgumentOutOfRangeException>(() => n.Sigma = -1.0);
        }

        /// <summary>
        /// Can set mu.
        /// </summary>
        /// <param name="mu">Mu parameter.</param>
        [TestCase(Double.NegativeInfinity)]
        [TestCase(-1.0)]
        [TestCase(-0.0)]
        [TestCase(0.0)]
        [TestCase(0.1)]
        [TestCase(1.0)]
        [TestCase(10.0)]
        [TestCase(Double.PositiveInfinity)]
        public void CanSetMu(double mu)
        {
            new LogNormal(1.0, 2.0)
            {
                Mu = mu
            };
        }

        /// <summary>
        /// Validate entropy.
        /// </summary>
        /// <param name="mu">Mu parameter.</param>
        /// <param name="sigma">Sigma value.</param>
        /// <param name="entropy">Expected value.</param>
        [TestCase(-1.000000, 0.100000, -1.8836465597893728867265104870209210873020761202386)]
        [TestCase(-1.000000, 1.500000, 0.82440364131283712375834285186996677643338789710028)]
        [TestCase(-1.000000, 2.500000, 1.335229265078827806963856948173628711311498693546)]
        [TestCase(-1.000000, 5.500000, 2.1236866254430979764250411929125703716076041932149)]
        [TestCase(-0.100000, 0.100000, -0.9836465597893728922776256101467037894202344606927)]
        [TestCase(-0.100000, 1.500000, 1.7244036413128371182072277287441840743152295566462)]
        [TestCase(-0.100000, 2.500000, 2.2352292650788278014127418250478460091933403530919)]
        [TestCase(-0.100000, 5.500000, 3.0236866254430979708739260697867876694894458527608)]
        [TestCase(0.100000, 0.100000, -0.7836465597893728811753953638951383851839177797845)]
        [TestCase(0.100000, 1.500000, 1.9244036413128371293094579749957494785515462375544)]
        [TestCase(0.100000, 2.500000, 2.4352292650788278125149720712994114134296570340001)]
        [TestCase(0.100000, 5.500000, 3.223686625443097981976156316038353073725762533669)]
        [TestCase(1.500000, 0.100000, 0.6163534402106271132734895129790789126979238797614)]
        [TestCase(1.500000, 1.500000, 3.3244036413128371237583428518699667764333878971003)]
        [TestCase(1.500000, 2.500000, 3.835229265078827806963856948173628711311498693546)]
        [TestCase(1.500000, 5.500000, 4.6236866254430979764250411929125703716076041932149)]
        [TestCase(2.500000, 0.100000, 1.6163534402106271132734895129790789126979238797614)]
        [TestCase(2.500000, 1.500000, 4.3244036413128371237583428518699667764333878971003)]
        [TestCase(2.500000, 2.500000, 4.835229265078827806963856948173628711311498693546)]
        [TestCase(2.500000, 5.500000, 5.6236866254430979764250411929125703716076041932149)]
        [TestCase(5.500000, 0.100000, 4.6163534402106271132734895129790789126979238797614)]
        [TestCase(5.500000, 1.500000, 7.3244036413128371237583428518699667764333878971003)]
        [TestCase(5.500000, 2.500000, 7.835229265078827806963856948173628711311498693546)]
        [TestCase(5.500000, 5.500000, 8.6236866254430979764250411929125703716076041932149)]
        [TestCase(3.0, 0.0, Double.NegativeInfinity)]
        public void ValidateEntropy(double mu, double sigma, double entropy)
        {
            var n = new LogNormal(mu, sigma);
            AssertHelpers.AlmostEqual(entropy, n.Entropy, 14);
        }

        /// <summary>
        /// Validate skewness.
        /// </summary>
        /// <param name="mu">Mu parameter.</param>
        /// <param name="sigma">Sigma value.</param>
        /// <param name="skewness">Expected value.</param>
        [TestCase(-1.000000, 0.100000, 0.30175909933883402945387113824982918009810212213629)]
        [TestCase(-1.000000, 1.500000, 33.46804679732172529147579024311650645764144530123)]
        [TestCase(-1.000000, 2.500000, 11824.007933610287521341659465200553739278936344799)]
        [TestCase(-1.000000, 5.500000, 50829064464591483629.132631635472412625371367420496)]
        [TestCase(-0.100000, 0.100000, 0.30175909933883402945387113824982918009810212213629)]
        [TestCase(-0.100000, 1.500000, 33.46804679732172529147579024311650645764144530123)]
        [TestCase(-0.100000, 2.500000, 11824.007933610287521341659465200553739278936344799)]
        [TestCase(-0.100000, 5.500000, 50829064464591483629.132631635472412625371367420496)]
        [TestCase(0.100000, 0.100000, 0.30175909933883402945387113824982918009810212213629)]
        [TestCase(0.100000, 1.500000, 33.46804679732172529147579024311650645764144530123)]
        [TestCase(0.100000, 2.500000, 11824.007933610287521341659465200553739278936344799)]
        [TestCase(0.100000, 5.500000, 50829064464591483629.132631635472412625371367420496)]
        [TestCase(1.500000, 0.100000, 0.30175909933883402945387113824982918009810212213629)]
        [TestCase(1.500000, 1.500000, 33.46804679732172529147579024311650645764144530123)]
        [TestCase(1.500000, 2.500000, 11824.007933610287521341659465200553739278936344799)]
        [TestCase(1.500000, 5.500000, 50829064464591483629.132631635472412625371367420496)]
        [TestCase(2.500000, 0.100000, 0.30175909933883402945387113824982918009810212213629)]
        [TestCase(2.500000, 1.500000, 33.46804679732172529147579024311650645764144530123)]
        [TestCase(2.500000, 2.500000, 11824.007933610287521341659465200553739278936344799)]
        [TestCase(2.500000, 5.500000, 50829064464591483629.132631635472412625371367420496)]
        [TestCase(5.500000, 0.100000, 0.30175909933883402945387113824982918009810212213629)]
        [TestCase(5.500000, 1.500000, 33.46804679732172529147579024311650645764144530123)]
        [TestCase(5.500000, 2.500000, 11824.007933610287521341659465200553739278936344799)]
        [TestCase(5.500000, 5.500000, 50829064464591483629.132631635472412625371367420496)]
        public void ValidateSkewness(double mu, double sigma, double skewness)
        {
            var n = new LogNormal(mu, sigma);
            AssertHelpers.AlmostEqual(skewness, n.Skewness, 14);
        }

        /// <summary>
        /// Validate mode.
        /// </summary>
        /// <param name="mu">Mu parameter.</param>
        /// <param name="sigma">Sigma value.</param>
        /// <param name="mode">Expected value.</param>
        [TestCase(-1.000000, 0.100000, 0.36421897957152331652213191863106773137983085909534)]
        [TestCase(-1.000000, 1.500000, 0.03877420783172200988689983526759614326014406193602)]
        [TestCase(-1.000000, 2.500000, 0.0007101743888425490635846003705775444086763023873619)]
        [TestCase(-1.000000, 5.500000, 0.000000000000026810038677818032221548731163905979029274677187036)]
        [TestCase(-0.100000, 0.100000, 0.89583413529652823774737070060865897390995185639633)]
        [TestCase(-0.100000, 1.500000, 0.095369162215549610417813418326627245539514227574881)]
        [TestCase(-0.100000, 2.500000, 0.0017467471362611196181003627521060283221112106850165)]
        [TestCase(-0.100000, 5.500000, 0.00000000000006594205454219929159167575814655534255162059017114)]
        [TestCase(0.100000, 0.100000, 1.0941742837052103542285651753780976842292770841345)]
        [TestCase(0.100000, 1.500000, 0.11648415777349696821514223131929465848700730137808)]
        [TestCase(0.100000, 2.500000, 0.0021334817700377079925027678518795817076296484352472)]
        [TestCase(0.100000, 5.500000, 0.000000000000080541807296590798973741710866097756565304960216803)]
        [TestCase(1.500000, 0.100000, 4.4370955190036645692996309927420381428715912422597)]
        [TestCase(1.500000, 1.500000, 0.47236655274101470713804655094326791297020357913648)]
        [TestCase(1.500000, 2.500000, 0.008651695203120634177071503957250390848166331197708)]
        [TestCase(1.500000, 5.500000, 0.00000000000032661313427874471360158184468030186601222739665225)]
        [TestCase(2.500000, 0.100000, 12.061276120444720299113038763305617245808510584994)]
        [TestCase(2.500000, 1.500000, 1.2840254166877414840734205680624364583362808652815)]
        [TestCase(2.500000, 2.500000, 0.023517745856009108236151185100432939470067655273072)]
        [TestCase(2.500000, 5.500000, 0.00000000000088782654784596584473099190326928541185172970391855)]
        [TestCase(5.500000, 0.100000, 242.2572068579541371904816252345031593584721473492)]
        [TestCase(5.500000, 1.500000, 25.790339917193062089080107669377221876655268848954)]
        [TestCase(5.500000, 2.500000, 0.47236655274101470713804655094326791297020357913648)]
        [TestCase(5.500000, 5.500000, 0.000000000017832472908146389493511850431527026413424899198327)]
        public void ValidateMode(double mu, double sigma, double mode)
        {
            var n = new LogNormal(mu, sigma);
            Assert.AreEqual(mode, n.Mode);
        }

        /// <summary>
        /// Validate median.
        /// </summary>
        /// <param name="mu">Mu parameter.</param>
        /// <param name="sigma">Sigma value.</param>
        /// <param name="median">Expected value.</param>
        [TestCase(-1.000000, 0.100000, 0.36787944117144232159552377016146086744581113103177)]
        [TestCase(-1.000000, 1.500000, 0.36787944117144232159552377016146086744581113103177)]
        [TestCase(-1.000000, 2.500000, 0.36787944117144232159552377016146086744581113103177)]
        [TestCase(-1.000000, 5.500000, 0.36787944117144232159552377016146086744581113103177)]
        [TestCase(-0.100000, 0.100000, 0.90483741803595956814139238421693559530906465375738)]
        [TestCase(-0.100000, 1.500000, 0.90483741803595956814139238421693559530906465375738)]
        [TestCase(-0.100000, 2.500000, 0.90483741803595956814139238421693559530906465375738)]
        [TestCase(-0.100000, 5.500000, 0.90483741803595956814139238421693559530906465375738)]
        [TestCase(0.100000, 0.100000, 1.1051709180756476309466388234587796577416634163742)]
        [TestCase(0.100000, 1.500000, 1.1051709180756476309466388234587796577416634163742)]
        [TestCase(0.100000, 2.500000, 1.1051709180756476309466388234587796577416634163742)]
        [TestCase(0.100000, 5.500000, 1.1051709180756476309466388234587796577416634163742)]
        [TestCase(1.500000, 0.100000, 4.4816890703380648226020554601192758190057498683697)]
        [TestCase(1.500000, 1.500000, 4.4816890703380648226020554601192758190057498683697)]
        [TestCase(1.500000, 2.500000, 4.4816890703380648226020554601192758190057498683697)]
        [TestCase(1.500000, 5.500000, 4.4816890703380648226020554601192758190057498683697)]
        [TestCase(2.500000, 0.100000, 12.182493960703473438070175951167966183182767790063)]
        [TestCase(2.500000, 1.500000, 12.182493960703473438070175951167966183182767790063)]
        [TestCase(2.500000, 2.500000, 12.182493960703473438070175951167966183182767790063)]
        [TestCase(2.500000, 5.500000, 12.182493960703473438070175951167966183182767790063)]
        [TestCase(5.500000, 0.100000, 244.6919322642203879151889495118393501842287101075)]
        [TestCase(5.500000, 1.500000, 244.6919322642203879151889495118393501842287101075)]
        [TestCase(5.500000, 2.500000, 244.6919322642203879151889495118393501842287101075)]
        [TestCase(5.500000, 5.500000, 244.6919322642203879151889495118393501842287101075)]
        public void ValidateMedian(double mu, double sigma, double median)
        {
            var n = new LogNormal(mu, sigma);
            Assert.AreEqual(median, n.Median);
        }

        /// <summary>
        /// Validate mean.
        /// </summary>
        /// <param name="mu">Mu parameter.</param>
        /// <param name="sigma">Sigma value.</param>
        /// <param name="mean">Expected value.</param>
        [TestCase(-1.000000, 0.100000, 0.36972344454405898424295931933535060663729727450496)]
        [TestCase(-1.000000, 1.500000, 1.1331484530668263168290072278117938725655031317452)]
        [TestCase(-1.000000, 2.500000, 8.3728974881272646632047051583699874196015291437918)]
        [TestCase(-1.000000, 5.500000, 1362729.1842528548177103892815156762190272224157908)]
        [TestCase(-0.100000, 0.100000, 0.90937293446823141948366366799116134283184493055232)]
        [TestCase(-0.100000, 1.500000, 2.7870954605658505209699655454000403395863724001622)]
        [TestCase(-0.100000, 2.500000, 20.594004711196027346218102453235151379866942184579)]
        [TestCase(-0.100000, 5.500000, 3351772.9412526949983798753257651403306685815830315)]
        [TestCase(0.100000, 0.100000, 1.1107106103557052433570611860384876269319432656698)]
        [TestCase(0.100000, 1.500000, 3.4041660827908192886708290528609320712960422205023)]
        [TestCase(0.100000, 2.500000, 25.153574155818364061848601838108180348672588964125)]
        [TestCase(0.100000, 5.500000, 4093864.7151726636524297378613262447736728507467499)]
        [TestCase(1.500000, 0.100000, 4.5041536302884836520306376113128094189800629942172)]
        [TestCase(1.500000, 1.500000, 13.804574186067094919261248628970575865946258844868)]
        [TestCase(1.500000, 2.500000, 102.00277308269968445339478193484494686013688925329)]
        [TestCase(1.500000, 5.500000, 16601440.057234774713918640507932346750889433699096)]
        [TestCase(2.500000, 0.100000, 12.243558965801025772304627735965552181680541950402)]
        [TestCase(2.500000, 1.500000, 37.524723159600998914070697772298569304087527691818)]
        [TestCase(2.500000, 2.500000, 277.27228452313398040814702091277144916631260200421)]
        [TestCase(2.500000, 5.500000, 45127392.833833379992911980630933945681066040228608)]
        [TestCase(5.500000, 0.100000, 245.91845567882191847293631456824227914641401674654)]
        [TestCase(5.500000, 1.500000, 753.70421255456126566058070133948176772966773355511)]
        [TestCase(5.500000, 2.500000, 5569.16270856600407442234466894967473356247174813)]
        [TestCase(5.500000, 5.500000, 906407915.01115491334464289369168840924937330105415)]
        public void ValidateMean(double mu, double sigma, double mean)
        {
            var n = new LogNormal(mu, sigma);
            AssertHelpers.AlmostEqual(mean, n.Mean, 14);
        }

        /// <summary>
        /// Validate minimum.
        /// </summary>
        [Test]
        public void ValidateMinimum()
        {
            var n = new LogNormal(1.0, 2.0);
            Assert.AreEqual(0.0, n.Minimum);
        }

        /// <summary>
        /// Validate maximum.
        /// </summary>
        [Test]
        public void ValidateMaximum()
        {
            var n = new LogNormal(1.0, 2.0);
            Assert.AreEqual(Double.PositiveInfinity, n.Maximum);
        }

        /// <summary>
        /// Validate density.
        /// </summary>
        /// <param name="mu">Mu parameter.</param>
        /// <param name="sigma">Sigma value.</param>
        /// <param name="x">Input X value.</param>
        /// <param name="p">Expected value.</param>
        [TestCase(-0.100000, 0.100000, -0.100000, 0.0)]
        [TestCase(-0.100000, 0.100000, 0.100000, 1.7968349035073582236359415565799753846986440127816e-104)]
        [TestCase(-0.100000, 0.100000, 0.500000, 0.00000018288923328441197822391757965928083462391836798722)]
        [TestCase(-0.100000, 0.100000, 0.800000, 2.3363114904470413709866234247494393485647978367885)]
        [TestCase(-0.100000, 1.500000, 0.100000, 0.90492497850024368541682348133921492204585092983646)]
        [TestCase(-0.100000, 1.500000, 0.500000, 0.49191985207660942803818797602364034466489243416574)]
        [TestCase(-0.100000, 1.500000, 0.800000, 0.33133347214343229148978298237579567194870525187207)]
        [TestCase(-0.100000, 2.500000, 0.100000, 1.0824698632626565182080576574958317806389057196768)]
        [TestCase(-0.100000, 2.500000, 0.500000, 0.31029619474753883558901295436486123689563749784867)]
        [TestCase(-0.100000, 2.500000, 0.800000, 0.19922929916156673799861939824205622734205083805245)]
        [TestCase(1.500000, 0.100000, 0.100000, 4.1070141770545881694056265342787422035256248474059e-313)]
        [TestCase(1.500000, 0.100000, 0.500000, 2.8602688726477103843476657332784045661507239533567e-104)]
        [TestCase(1.500000, 0.100000, 0.800000, 1.6670425710002183246335601541889400558525870482613e-64)]
        [TestCase(1.500000, 1.500000, 0.100000, 0.10698412103361841220076392503406214751353235895732)]
        [TestCase(1.500000, 1.500000, 0.500000, 0.18266125308224685664142384493330155315630876975024)]
        [TestCase(1.500000, 1.500000, 0.800000, 0.17185785323404088913982425377565512294017306418953)]
        [TestCase(1.500000, 2.500000, 0.100000, 0.50186885259059181992025035649158160252576845315332)]
        [TestCase(1.500000, 2.500000, 0.500000, 0.21721369314437986034957451699565540205404697589349)]
        [TestCase(1.500000, 2.500000, 0.800000, 0.15729636000661278918949298391170443742675565300598)]
        [TestCase(2.500000, 0.100000, 0.100000, 5.6836826548848916385760779034504046896805825555997e-500)]
        [TestCase(2.500000, 0.100000, 0.500000, 3.1225608678589488061206338085285607881363155340377e-221)]
        [TestCase(2.500000, 0.100000, 0.800000, 4.6994713794671660918554320071312374073172560048297e-161)]
        [TestCase(2.500000, 1.500000, 0.100000, 0.015806486291412916772431170442330946677601577502353)]
        [TestCase(2.500000, 1.500000, 0.500000, 0.055184331257528847223852028950484131834529030116388)]
        [TestCase(2.500000, 1.500000, 0.800000, 0.063982134749859504449658286955049840393511776984362)]
        [TestCase(2.500000, 2.500000, 0.100000, 0.25212505662402617595900822552548977822542300480086)]
        [TestCase(2.500000, 2.500000, 0.500000, 0.14117186955911792460646517002386088579088567275401)]
        [TestCase(2.500000, 2.500000, 0.800000, 0.11021452580363707866161369621432656293405065561317)]
        public void ValidateDensity(double mu, double sigma, double x, double p)
        {
            var n = new LogNormal(mu, sigma);
            AssertHelpers.AlmostEqual(p, n.Density(x), 14);
        }

        /// <summary>
        /// Validate density log.
        /// </summary>
        /// <param name="mu">Mu parameter.</param>
        /// <param name="sigma">Sigma value.</param>
        /// <param name="x">Input X value.</param>
        /// <param name="p">Expected value.</param>
        [TestCase(-0.100000, 0.100000, -0.100000, Double.NegativeInfinity)]
        [TestCase(-0.100000, 0.100000, 0.100000, -238.88282294119596467794686179588610665317241097599)]
        [TestCase(-0.100000, 0.100000, 0.500000, -15.514385149961296196003163062199569075052113039686)]
        [TestCase(-0.100000, 0.100000, 0.800000, 0.84857339958981283964373051826407417105725729082041)]
        [TestCase(-0.100000, 1.500000, 0.100000, -0.099903235403144611051953094864849327288457482212211)]
        [TestCase(-0.100000, 1.500000, 0.500000, -0.70943947804316122682964396008813828577195771418027)]
        [TestCase(-0.100000, 1.500000, 0.800000, -1.1046299420497998262946038709903250420774183529995)]
        [TestCase(-0.100000, 2.500000, 0.100000, 0.07924534056485078867266307735371665927517517183681)]
        [TestCase(-0.100000, 2.500000, 0.500000, -1.1702279707433794860424967893989374511050637417043)]
        [TestCase(-0.100000, 2.500000, 0.800000, -1.6132988605030400828957768752511536087538109996183)]
        [TestCase(1.500000, 0.100000, 0.100000, -719.29643782024317312262673764204041218720576249741)]
        [TestCase(1.500000, 0.100000, 0.500000, -238.41793403955250272430898754048547661932857086122)]
        [TestCase(1.500000, 0.100000, 0.800000, -146.85439481068371057247137024006716189469284256628)]
        [TestCase(1.500000, 1.500000, 0.100000, -2.2350748570877992856465076624973458117562108140674)]
        [TestCase(1.500000, 1.500000, 0.500000, -1.7001219175524556705452882616787223585705662860012)]
        [TestCase(1.500000, 1.500000, 0.800000, -1.7610875785399045023354101841009649273236721172008)]
        [TestCase(1.500000, 2.500000, 0.100000, -0.68941644324162489418137656699398207513321602763104)]
        [TestCase(1.500000, 2.500000, 0.500000, -1.5268736489667254857801287379715477173125628275598)]
        [TestCase(1.500000, 2.500000, 0.800000, -1.8496236096394777662704671479709839674424623547308)]
        [TestCase(2.500000, 0.100000, 0.100000, -1149.5549471196476523788026360929146688367845019398)]
        [TestCase(2.500000, 0.100000, 0.500000, -507.73265209554698134113704985174959301922196605736)]
        [TestCase(2.500000, 0.100000, 0.800000, -369.16874994210463740474549611573497379941224077335)]
        [TestCase(2.500000, 1.500000, 0.100000, -4.1473348984184862316495477617980296904955324113457)]
        [TestCase(2.500000, 1.500000, 0.500000, -2.8970762200235424747307247601045786110485663457169)]
        [TestCase(2.500000, 1.500000, 0.800000, -2.7491513791239977024488074547907467152956602019989)]
        [TestCase(2.500000, 2.500000, 0.100000, -1.3778300581206721947424710027422282714793718026513)]
        [TestCase(2.500000, 2.500000, 0.500000, -1.9577771978563167352868858774048559682046428490575)]
        [TestCase(2.500000, 2.500000, 0.800000, -2.2053265778497513183112901654193054111123780652581)]
        public void ValidateDensityLn(double mu, double sigma, double x, double p)
        {
            var n = new LogNormal(mu, sigma);
            AssertHelpers.AlmostEqual(p, n.DensityLn(x), 14);
        }

        /// <summary>
        /// Can sample static.
        /// </summary>
        [Test]
        public void CanSampleStatic()
        {
            LogNormal.Sample(new Random(), 0.0, 1.0);
        }

        /// <summary>
        /// Can sample sequence static.
        /// </summary>
        [Test]
        public void CanSampleSequenceStatic()
        {
            var ied = LogNormal.Samples(new Random(), 0.0, 1.0);
            ied.Take(5).ToArray();
        }

        /// <summary>
        /// Fail sample static with bad parameters.
        /// </summary>
        [Test]
        public void FailSampleStatic()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => { var d = LogNormal.Sample(new Random(), 0.0, -1.0); });
        }

        /// <summary>
        /// Fail sample sequence static with bad parameters.
        /// </summary>
        [Test]
        public void FailSampleSequenceStatic()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => { var ied = LogNormal.Samples(new Random(), 0.0, -1.0).First(); });
        }

        /// <summary>
        /// Can sample.
        /// </summary>
        [Test]
        public void CanSample()
        {
            var n = new LogNormal(1.0, 2.0);
            n.Sample();
        }

        /// <summary>
        /// Can sample sequence.
        /// </summary>
        [Test]
        public void CanSampleSequence()
        {
            var n = new LogNormal(1.0, 2.0);
            var ied = n.Samples();
            ied.Take(5).ToArray();
        }

        /// <summary>
        /// Validate cumulative distribution.
        /// </summary>
        /// <param name="mu">Mu parameter.</param>
        /// <param name="sigma">Sigma value.</param>
        /// <param name="x">Input X value.</param>
        /// <param name="f">Expected value.</param>
        [TestCase(-0.100000, 0.100000, -0.100000, 0.0)]
        [TestCase(-0.100000, 0.100000, 0.100000, 0.0)]
        [TestCase(-0.100000, 0.100000, 0.500000, 0.0000000015011556178148777579869633555518882664666520593658)]
        [TestCase(-0.100000, 0.100000, 0.800000, 0.10908001076375810900224507908874442583171381706127)]
        [TestCase(-0.100000, 1.500000, 0.100000, 0.070999149762464508991968731574953594549291668468349)]
        [TestCase(-0.100000, 1.500000, 0.500000, 0.34626224992888089297789445771047690175505847991946)]
        [TestCase(-0.100000, 1.500000, 0.800000, 0.46728530589487698517090261668589508746353129242404)]
        [TestCase(-0.100000, 2.500000, 0.100000, 0.18914969879695093477606645992572208111152994999076)]
        [TestCase(-0.100000, 2.500000, 0.500000, 0.40622798321378106125020505907901206714868922279347)]
        [TestCase(-0.100000, 2.500000, 0.800000, 0.48035707589956665425068652807400957345208517749893)]
        [TestCase(1.500000, 0.100000, 0.100000, 0.0)]
        [TestCase(1.500000, 0.100000, 0.500000, 0.0)]
        [TestCase(1.500000, 0.100000, 0.800000, 0.0)]
        [TestCase(1.500000, 1.500000, 0.100000, 0.005621455876973168709588070988239748831823850202953)]
        [TestCase(1.500000, 1.500000, 0.500000, 0.07185716187918271235246980951571040808235628115265)]
        [TestCase(1.500000, 1.500000, 0.800000, 0.12532699044614938400496547188720940854423187977236)]
        [TestCase(1.500000, 2.500000, 0.100000, 0.064125647996943514411570834861724406903677144126117)]
        [TestCase(1.500000, 2.500000, 0.500000, 0.19017302281590810871719754032332631806011441356498)]
        [TestCase(1.500000, 2.500000, 0.800000, 0.24533064397555500690927047163085419096928289095201)]
        [TestCase(2.500000, 0.100000, 0.100000, 0.0)]
        [TestCase(2.500000, 0.100000, 0.500000, 0.0)]
        [TestCase(2.500000, 0.100000, 0.800000, 0.0)]
        [TestCase(2.500000, 1.500000, 0.100000, 0.00068304052220788502001572635016579586444611070077399)]
        [TestCase(2.500000, 1.500000, 0.500000, 0.016636862816580533038130583128179878924863968664206)]
        [TestCase(2.500000, 1.500000, 0.800000, 0.034729001282904174941366974418836262996834852343018)]
        [TestCase(2.500000, 2.500000, 0.100000, 0.027363708266690978870139978537188410215717307180775)]
        [TestCase(2.500000, 2.500000, 0.500000, 0.10075543423327634536450625420610429181921642201567)]
        [TestCase(2.500000, 2.500000, 0.800000, 0.13802019192453118732001307556787218421918336849121)]
        public void ValidateCumulativeDistribution(double mu, double sigma, double x, double f)
        {
            var n = new LogNormal(mu, sigma);
            AssertHelpers.AlmostEqual(f, n.CumulativeDistribution(x), 8);
        }

        /// <summary>
        /// Can estimate distribution parameters.
        /// </summary>
        [TestCase(0.0, 0.0)]
        [TestCase(10.0, 0.1)]
        [TestCase(-5.0, 1.0)]
        [TestCase(0.0, 5.0)]
        [TestCase(10.0, 50.0)]
        public void CanEstimateParameters(double mu, double sigma)
        {
            var original = new LogNormal(mu, sigma, new Random(100));
            var estimated = LogNormal.Estimate(original.Samples().Take(10000));

            AssertHelpers.AlmostEqual(mu, estimated.Mu, 2);
            AssertHelpers.AlmostEqual(sigma, estimated.Sigma, 2);
        }
    }
}
