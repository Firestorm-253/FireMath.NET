namespace FireMath.NET.Distributions;

public static class GaussianElo
{
    static readonly double ROOT_2 = 2.0.Sqrt();

    // var rating_a = Gaussian.ByMeanDeviation(1200, 5);
    // var rating_b = Gaussian.ByMeanDeviation(1000, 600);
    public static (Gaussian, Gaussian) GetRatingsAfter(Gaussian rating_a, Gaussian rating_b, int actualResult_a)
    {
        var (winChance_a, matchDist) = PredictMatch(rating_a, rating_b);

        var match_goal = Gaussian.ByMeanDeviation(-matchDist.Deviation, (ROOT_2 * matchDist.Deviation) /*(match.Deviation.Pow() + match.Deviation.Pow()).Sqrt()*/);
        var delta = (matchDist.Mean - match_goal.Mean) / 2;

        var delta_a = (actualResult_a == 1) ? delta : -delta;
        var delta_b = (actualResult_a == 0) ? delta : -delta;

        var info_a = Gaussian.ByMeanDeviation(rating_a.Mean + delta_a, matchDist.Deviation);
        var info_b = Gaussian.ByMeanDeviation(rating_b.Mean + delta_b, matchDist.Deviation);
        //var (winChance_info, matchGaussian_info) = GetWinChance(info_a, info_b);

        var ratingAfter_a = rating_a * info_a;
        var ratingAfter_b = rating_b * info_b;
        //var (winChanceAfter, matchGaussianAfter) = GetWinChance(ratingAfter_a, ratingAfter_b);

        return (ratingAfter_a, ratingAfter_b);
    }

    public static Gaussian GetRatingAfter(Gaussian rating, int actualResult, Gaussian matchDist)
    {
        double prediction = matchDist.CDF(0);
        double delta = (actualResult - prediction) * (1 - rating.Precision) * matchDist.Precision;

        var info = Gaussian.ByMeanDeviation(rating.Mean + delta, matchDist.Deviation);
        var ratingAfter = rating * info;

        if ((prediction - actualResult).Abs() >= 0.5)
        {
            ratingAfter = Gaussian.ByMeanDeviation(ratingAfter.Mean, rating.Deviation + (ratingAfter.Deviation - rating.Deviation).Abs());
        }

        return ratingAfter;
    }

    public static (double, Gaussian) PredictMatch(Gaussian a, Gaussian b)
    {
        var subtraction = b - a;
        return (subtraction.CDF(0), subtraction);
    }
}
