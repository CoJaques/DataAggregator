using DataAggregator.Processor.Services.Processing.PreProcessing.ActuatorMergingCurrentPreprocessing;

namespace DataAggregator.Processor.Tests.Services.PreProcessing;

/// <summary>
/// Tests for the <see cref="MathUtils"/> class.
/// </summary>
public class MathUtilsTests
{
    #region Mean tests
    [Fact]
    public void Mean_ShouldReturnZero_WhenValuesIsEmpty()
    {
        // Arrange
        var values = new List<float>();

        // Act
        float result = MathUtils.Mean(values);

        // Assert
        Assert.Equal(0.0f, result);
    }

    [Fact]
    public void Mean_ShouldReturnCorrectValue_WhenValuesContainsSingleElement()
    {
        // Arrange
        var values = new List<float> { 5.0f };

        // Act
        float result = MathUtils.Mean(values);

        // Assert
        Assert.Equal(5.0f, result);
    }

    [Fact]
    public void Mean_ShouldReturnCorrectValue_WhenValuesContainsMultipleElements()
    {
        // Arrange
        var values = new List<float> { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };

        // Act
        float result = MathUtils.Mean(values);

        // Assert
        Assert.Equal(3.0f, result);
    }

    #endregion

    #region StandardDeviation tests
    [Fact]
    public void StandardDeviation_ShouldReturnZero_WhenValuesIsEmpty()
    {
        // Arrange
        var values = new List<float>();

        // Act
        float result = MathUtils.StandardDeviation(values);

        // Assert
        Assert.Equal(0.0f, result);
    }

    [Fact]
    public void StandardDeviation_ShouldReturnZero_WhenValuesContainsSingleElement()
    {
        // Arrange
        var values = new List<float> { 5.0f };

        // Act
        float result = MathUtils.StandardDeviation(values);

        // Assert
        Assert.Equal(0.0f, result);
    }

    [Fact]
    public void StandardDeviation_ShouldReturnCorrectValue_WhenValuesContainsMultipleElements()
    {
        // Arrange
        var values = new List<float> { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };

        // Act
        float result = MathUtils.StandardDeviation(values);

        // Assert
        // Expected: sqrt(sum((x - mean)^2) / n) = sqrt(10 / 5) = sqrt(2) ≈ 1.414
        Assert.Equal(1.4142135f, result, 6);
    }

    [Fact]
    public void StandardDeviation_ShouldReturnZero_WhenAllValuesAreIdentical()
    {
        // Arrange
        var values = new List<float> { 3.0f, 3.0f, 3.0f, 3.0f };

        // Act
        float result = MathUtils.StandardDeviation(values);

        // Assert
        Assert.Equal(0.0f, result);
    }

    #endregion

    #region Percentile tests

    [Fact]
    public void Percentile_ShouldReturnZero_WhenValuesIsEmpty()
    {
        // Arrange
        var values = new List<float>();

        // Act
        float result = MathUtils.Percentile(values, 50.0f);

        // Assert
        Assert.Equal(0.0f, result);
    }

    [Fact]
    public void Percentile_ShouldReturnCorrectValue_WhenPercentileIs0()
    {
        // Arrange
        var values = new List<float> { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };

        // Act
        float result = MathUtils.Percentile(values, 0.0f);

        // Assert
        Assert.Equal(1.0f, result);
    }

    [Fact]
    public void Percentile_ShouldReturnCorrectValue_WhenPercentileIs50()
    {
        // Arrange
        var values = new List<float> { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };

        // Act
        float result = MathUtils.Percentile(values, 50.0f);

        // Assert
        Assert.Equal(3.0f, result);
    }

    [Fact]
    public void Percentile_ShouldReturnCorrectValue_WhenPercentileIs100()
    {
        // Arrange
        var values = new List<float> { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };

        // Act
        float result = MathUtils.Percentile(values, 100.0f);

        // Assert
        Assert.Equal(5.0f, result);
    }

    [Fact]
    public void Percentile_ShouldReturnCorrectValue_WhenPercentileIs25()
    {
        // Arrange
        var values = new List<float> { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };

        // Act
        float result = MathUtils.Percentile(values, 25.0f);

        // Assert
        // For 5 elements, 25th percentile should be the 2nd element (index 1)
        Assert.Equal(2.0f, result);
    }

    [Fact]
    public void Percentile_ShouldReturnCorrectValue_WhenPercentileIs75()
    {
        // Arrange
        var values = new List<float> { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };

        // Act
        float result = MathUtils.Percentile(values, 75.0f);

        // Assert
        // For 5 elements, 75th percentile should be the 4th element (index 3)
        Assert.Equal(4.0f, result);
    }

    #endregion

    #region Skewness tests

    [Fact]
    public void Skewness_ShouldReturnZero_WhenValuesContainsLessThan3Elements()
    {
        // Arrange
        var values = new List<float> { 1.0f, 2.0f };

        // Act
        float result = MathUtils.Skewness(values);

        // Assert
        Assert.Equal(0.0f, result);
    }

    [Fact]
    public void Skewness_ShouldReturnZero_WhenStandardDeviationIsZero()
    {
        // Arrange
        var values = new List<float> { 3.0f, 3.0f, 3.0f };

        // Act
        float result = MathUtils.Skewness(values);

        // Assert
        Assert.Equal(0.0f, result);
    }

    [Fact]
    public void Skewness_ShouldReturnCorrectValue_WhenValuesAreSymmetric()
    {
        // Arrange
        var values = new List<float> { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };

        // Act
        float result = MathUtils.Skewness(values);

        // Assert
        // For symmetric data around mean, skewness should be close to 0
        Assert.Equal(0.0f, result, 6);
    }

    [Fact]
    public void Skewness_ShouldReturnPositiveValue_WhenValuesAreRightSkewed()
    {
        // Arrange
        var values = new List<float> { 1.0f, 1.0f, 1.0f, 2.0f, 10.0f };

        // Act
        float result = MathUtils.Skewness(values);

        // Assert
        // Right-skewed data should have positive skewness
        Assert.True(result > 0.0f);
    }

    #endregion

    #region Correlation tests

    [Fact]
    public void Correlation_ShouldReturnZero_WhenXIsEmpty()
    {
        // Arrange
        var x = new List<float>();
        var y = new List<float> { 1.0f, 2.0f, 3.0f };

        // Act
        float result = MathUtils.Correlation(x, y);

        // Assert
        Assert.Equal(0.0f, result);
    }

    [Fact]
    public void Correlation_ShouldReturnZero_WhenYIsEmpty()
    {
        // Arrange
        var x = new List<float> { 1.0f, 2.0f, 3.0f };
        var y = new List<float>();

        // Act
        float result = MathUtils.Correlation(x, y);

        // Assert
        Assert.Equal(0.0f, result);
    }

    [Fact]
    public void Correlation_ShouldReturnZero_WhenLengthsAreDifferent()
    {
        // Arrange
        var x = new List<float> { 1.0f, 2.0f, 3.0f };
        var y = new List<float> { 1.0f, 2.0f };

        // Act
        float result = MathUtils.Correlation(x, y);

        // Assert
        Assert.Equal(0.0f, result);
    }

    [Fact]
    public void Correlation_ShouldReturnZero_WhenLengthIsLessThan2()
    {
        // Arrange
        var x = new List<float> { 1.0f };
        var y = new List<float> { 2.0f };

        // Act
        float result = MathUtils.Correlation(x, y);

        // Assert
        Assert.Equal(0.0f, result);
    }

    [Fact]
    public void Correlation_ShouldReturnOne_WhenPerfectPositiveCorrelation()
    {
        // Arrange
        var x = new List<float> { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };
        var y = new List<float> { 2.0f, 4.0f, 6.0f, 8.0f, 10.0f };

        // Act
        float result = MathUtils.Correlation(x, y);

        // Assert
        Assert.Equal(1.0f, result, 6);
    }

    [Fact]
    public void Correlation_ShouldReturnMinusOne_WhenPerfectNegativeCorrelation()
    {
        // Arrange
        var x = new List<float> { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };
        var y = new List<float> { 10.0f, 8.0f, 6.0f, 4.0f, 2.0f };

        // Act
        float result = MathUtils.Correlation(x, y);

        // Assert
        Assert.Equal(-1.0f, result, 6);
    }

    [Fact]
    public void Correlation_ShouldReturnZero_WhenNoCorrelation()
    {
        // Arrange
        var x = new List<float> { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };
        var y = new List<float> { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };

        // Act
        float result = MathUtils.Correlation(x, y);

        // Assert
        Assert.Equal(0.0f, result, 6);
    }

    [Fact]
    public void Correlation_ShouldReturnCorrectValue_WhenModerateCorrelation()
    {
        // Arrange
        var x = new List<float> { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };
        var y = new List<float> { 1.0f, 3.0f, 2.0f, 5.0f, 4.0f };

        // Act
        float result = MathUtils.Correlation(x, y);

        // Assert
        // Should be a moderate positive correlation
        Assert.True(result is > 0.0f and < 1.0f);
    }

    #endregion
}
