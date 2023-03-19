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
        Win_AsStronger(out double delta_asStronger);
        Win_AsWeaker(out double delta_asWeaker);

        Assert.Greater(delta_asWeaker, delta_asStronger);
    }
    [TestMethod]
    public void Loss()
    {
        Loss_AsStronger(out double delta_asStronger);
        Loss_AsWeaker(out double delta_asWeaker);

        Assert.Less(delta_asWeaker.Abs(), delta_asStronger.Abs());
    }

    [TestMethod]
    public void Win_AsStronger()
    {
        Win_AsStronger(out _);
    }
    [TestMethod]
    public void Win_AsWeaker()
    {
        Win_AsWeaker(out _);
    }

    [TestMethod]
    public void Loss_AsStronger()
    {
        Loss_AsStronger(out _);
    }
    [TestMethod]
    public void Loss_AsWeaker()
    {
        Loss_AsWeaker(out _);
    }


    public static void Win_AsStronger(out double delta)
    {
        var ratingBefore = Gaussian.ByMeanDeviation(1100, random.Next(1, 1000));
        var ratingOpp = Gaussian.ByMeanDeviation(1000, random.Next(1, 1000));

        var ratingAfter = Win(ratingBefore, ratingOpp, out var matchDist);
        delta = ratingAfter.Mean - ratingBefore.Mean;

        Assert.True(matchDist.Mean < 0);
    }
    public static void Win_AsWeaker(out double delta)
    {
        var ratingBefore = Gaussian.ByMeanDeviation(1000, random.Next(1, 1000));
        var ratingOpp = Gaussian.ByMeanDeviation(1100, random.Next(1, 1000));

        var ratingAfter = Win(ratingBefore, ratingOpp, out var matchDist);
        delta = ratingAfter.Mean - ratingBefore.Mean;

        Assert.True(matchDist.Mean > 0);
    }

    public static void Loss_AsStronger(out double delta)
    {
        var ratingBefore = Gaussian.ByMeanDeviation(1100, random.Next(1, 1000));
        var ratingOpp = Gaussian.ByMeanDeviation(1000, random.Next(1, 1000));

        var ratingAfter = Loss(ratingBefore, ratingOpp, out var matchDist);
        delta = ratingAfter.Mean - ratingBefore.Mean;

        Assert.True(matchDist.Mean < 0);
    }
    public static void Loss_AsWeaker(out double delta)
    {
        var ratingBefore = Gaussian.ByMeanDeviation(1000, random.Next(1, 1000));
        var ratingOpp = Gaussian.ByMeanDeviation(1100, random.Next(1, 1000));

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