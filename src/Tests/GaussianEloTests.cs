using FireMath.NET;
using FireMath.NET.Distributions;
using MathNet.Numerics.Distributions;
using NUnit.Framework;
using System.Diagnostics;
using Assert = NUnit.Framework.Assert;

namespace Tests;

[TestClass]
public class GaussianEloTests
{
    static Random random = new();

    [TestMethod]
    public void Win_Equal()
    {
        var ratingBefore = Gaussian.ByMeanDeviation(1000, random.Next(1, 1000));
        var ratingOpp = Gaussian.ByMeanDeviation(1000, random.Next(1, 1000));

        Win(ratingBefore, ratingOpp, out var matchDist);

        double winChance = matchDist.CDF(0);
        Assert.True(winChance == 0.5);
    }

    [TestMethod]
    public void Loss_Equal()
    {
        var ratingBefore = Gaussian.ByMeanDeviation(1000, random.Next(1, 1000));
        var ratingOpp = Gaussian.ByMeanDeviation(1000, random.Next(1, 1000));

        Loss(ratingBefore, ratingOpp, out var matchDist);

        double winChance = matchDist.CDF(0);
        Assert.True(winChance == 0.5);
    }

    [TestMethod]
    public void Win()
    {
        var deviations = (random.Next(1, 1000), random.Next(1, 1000));

        Win_AsStronger(out double delta_asStronger, deviations);
        Win_AsWeaker(out double delta_asWeaker, deviations);

        Assert.Greater(delta_asWeaker, delta_asStronger);
    }
    [TestMethod]
    public void Loss()
    {
        var deviations = (random.Next(1, 1000), random.Next(1, 1000));

        Loss_AsStronger(out double delta_asStronger, deviations);
        Loss_AsWeaker(out double delta_asWeaker, deviations);

        Assert.Less(delta_asWeaker.Abs(), delta_asStronger.Abs());
    }

    [TestMethod]
    public void Win_AsStronger()
    {
        Win_AsStronger(out _, (random.Next(1, 1000), random.Next(1, 1000)));
    }
    [TestMethod]
    public void Win_AsWeaker()
    {
        Win_AsWeaker(out _, (random.Next(1, 1000), random.Next(1, 1000)));
    }

    [TestMethod]
    public void Loss_AsStronger()
    {
        Loss_AsStronger(out _, (random.Next(1, 1000), random.Next(1, 1000)));
    }
    [TestMethod]
    public void Loss_AsWeaker()
    {
        Loss_AsWeaker(out _, (random.Next(1, 1000), random.Next(1, 1000)));
    }


    public static void Win_AsStronger(out double delta, (double, double) deviations)
    {
        var ratingBefore = Gaussian.ByMeanDeviation(1100, deviations.Item1);
        var ratingOpp = Gaussian.ByMeanDeviation(1000, deviations.Item2);

        var ratingAfter = Win(ratingBefore, ratingOpp, out var matchDist);
        delta = ratingAfter.Mean - ratingBefore.Mean;

        Assert.True(matchDist.Mean < 0);
    }
    public static void Win_AsWeaker(out double delta, (double, double) deviations)
    {
        var ratingBefore = Gaussian.ByMeanDeviation(1000, deviations.Item1);
        var ratingOpp = Gaussian.ByMeanDeviation(1100, deviations.Item2);

        var ratingAfter = Win(ratingBefore, ratingOpp, out var matchDist);
        delta = ratingAfter.Mean - ratingBefore.Mean;

        Assert.True(matchDist.Mean > 0);
    }

    public static void Loss_AsStronger(out double delta, (double, double) deviations)
    {
        var ratingBefore = Gaussian.ByMeanDeviation(1100, deviations.Item1);
        var ratingOpp = Gaussian.ByMeanDeviation(1000, deviations.Item2);

        var ratingAfter = Loss(ratingBefore, ratingOpp, out var matchDist);
        delta = ratingAfter.Mean - ratingBefore.Mean;

        Assert.True(matchDist.Mean < 0);
    }
    public static void Loss_AsWeaker(out double delta, (double, double) deviations)
    {
        var ratingBefore = Gaussian.ByMeanDeviation(1000, deviations.Item1);
        var ratingOpp = Gaussian.ByMeanDeviation(1100, deviations.Item2);

        var ratingAfter = Loss(ratingBefore, ratingOpp, out var matchDist);
        delta = ratingAfter.Mean - ratingBefore.Mean;

        Assert.True(matchDist.Mean > 0);
    }

    public static Gaussian Win(Gaussian ratingBefore, Gaussian ratingOpp, out Gaussian matchDist)
    {
        int actualResult = 1;
        var ratingAfter = Process(ratingBefore, ratingOpp, actualResult, out matchDist);

        Assert.Greater(ratingAfter.Mean, ratingBefore.Mean);

        return ratingAfter;
    }
    public static Gaussian Loss(Gaussian ratingBefore, Gaussian ratingOpp, out Gaussian matchDist)
    {
        int actualResult = 0;
        var ratingAfter = Process(ratingBefore, ratingOpp, actualResult, out matchDist);

        Assert.Less(ratingAfter.Mean, ratingBefore.Mean);

        return ratingAfter;
    }

    public static Gaussian Process(Gaussian ratingBefore, Gaussian ratingOpp, int actualResult, out Gaussian matchDist)
    {
        (_, matchDist) = FireMath.NET.Distributions.GaussianElo.PredictMatch(ratingBefore, ratingOpp);
        var ratingAfter = FireMath.NET.Distributions.GaussianElo.GetRatingAfter(ratingBefore, actualResult, matchDist);
        return ratingAfter;
    }
}
