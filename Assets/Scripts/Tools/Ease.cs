using UnityEngine;

/**
 * Static class with useful easer functions that can be used by Tweens.
 */
public class Ease
{
    /** Quadratic in. */
    public static float QuadIn(float t)
	{
		return t* t;
    }

    /** Quadratic out. */
    public static float QuadOut(float t)
	{
		return -t* (t - 2f);
	}

	/** Quadratic in and out. */
	public static float QuadInOut(float t)
	{
		return t <= 0.5f ? t * t * 2f : 1f - (--t) * t * 2f;
	}

	/** Cubic in. */
	public static float CubeIn(float t)
	{
		return t* t * t;
	}

	/** Cubic out. */
	public static float CubeOut(float t)
	{
		return 1f + (--t) * t * t;
	}

	/** Cubic in and out. */
	public static float CubeInOut(float t)
	{
		return t <= 0.5f ? t * t * t * 4f : 1f + (--t) * t * t * 4f;
	}

	/** Quart in. */
	public static float QuartIn(float t)
	{
		return t* t * t * t;
	}

	/** Quart out. */
	public static float QuartOut(float t)
	{
		return 1 - (t-=1f) * t * t * t;
	}

	/** Quart in and out. */
	public static float QuartInOut(float t)
	{
		return t <= 0.5f ? t * t * t * t * 8f : (1f - (t = t* 2f - 2f) * t * t * t) / 2f + 0.5f;
	}

	/** Quint in. */
	public static float QuintIn(float t)
	{
		return t* t * t * t * t;
	}

	/** Quint out. */
	public static float QuintOut(float t)
	{
		return (t = t - 1f) * t * t * t * t + 1f;
	}

	/** Quint in and out. */
	public static float QuintInOut(float t)
	{
		return ((t *= 2f) < 1f) ? (t* t * t * t * t) / 2f : ((t -= 2f) * t * t * t * t + 2f) / 2f;
	}

	/** Sine in. */
	public static float SineIn(float t)
	{
		return -Mathf.Cos(Mathf.PI * 2f * t) + 1f;
	}

	/** Sine out. */
	public static float SineOut(float t)
	{
		return Mathf.Sin(Mathf.PI * 2f * t);
	}

	/** Sine in and out. */
	public static float SineInOut(float t)
	{
		return -Mathf.Cos(Mathf.PI * t) / 2f + 0.5f;
	}

	/** Bounce in. */
	public static float BounceIn(float t)
	{
		t = 1f - t;
		if (t<B1) return 1f - 7.5625f * t* t;
		if (t<B2) return 1f - (7.5625f * (t - B3) * (t - B3) + 0.75f);
		if (t<B4) return 1f - (7.5625f * (t - B5) * (t - B5) + 0.9375f);
		return 1f - (7.5625f * (t - B6) * (t - B6) + 0.984375f);
	}

	/** Bounce out. */
	public static float BounceOut(float t)
	{
		if (t<B1) return 7.5625f * t* t;
		if (t<B2) return 7.5625f * (t - B3) * (t - B3) + 0.75f;
		if (t<B4) return 7.5625f * (t - B5) * (t - B5) + 0.9375f;
		return 7.5625f * (t - B6) * (t - B6) + 0.984375f;
	}

	/** Bounce in and out. */
	public static float BounceInOut(float t)
	{
		if (t< 0.5f)
		{
			t = 1f - t* 2f;
			if (t<B1) return (1f - 7.5625f * t * t) / 2f;
			if (t<B2) return (1f - (7.5625f * (t - B3) * (t - B3) + 0.75f)) / 2f;
			if (t<B4) return (1f - (7.5625f * (t - B5) * (t - B5) + 0.9375f)) / 2f;
			return (1f - (7.5625f * (t - B6) * (t - B6) + 0.984375f)) / 2f;
		}
		t = t* 2f - 1f;
		if (t<B1) return (7.5625f * t * t) / 2f + 0.5f;
		if (t<B2) return (7.5625f * (t - B3) * (t - B3) + 0.75f) / 2f + 0.5f;
		if (t<B4) return (7.5625f * (t - B5) * (t - B5) + 0.9375f) / 2f + 0.5f;
		return (7.5625f * (t - B6) * (t - B6) + .984375f) / 2f + 0.5f;
	}

	/** Circle in. */
	public static float CircIn(float t)
	{
		return -(Mathf.Sqrt(1f - t* t) - 1f);
	}

	/** Circle out. */
	public static float CircOut(float t)
	{
		return Mathf.Sqrt(1f - (t - 1f) * (t - 1f));
	}

	/** Circle in and out. */
	public static float CircInOut(float t)
	{
		return t <= 0.5f ? (Mathf.Sqrt(1f - t* t * 4f) - 1f) / -2f : (Mathf.Sqrt(1f - (t* 2f - 2f) * (t* 2f - 2f)) + 1f) / 2f;
	}

	/** Exponential in. */
	public static float ExpoIn(float t)
	{
		return Mathf.Pow(2f, 10 * (t - 1));
	}

	/** Exponential out. */
	public static float ExpoOut(float t)
	{
		return -Mathf.Pow(2f, -10 * t) + 1;
	}

	/** Exponential in and out. */
	public static float ExpoInOut(float t)
	{
		return t< 0.5f ? Mathf.Pow(2f, 10 * (t* 2 - 1)) / 2f : (-Mathf.Pow(2f, -10 * (t* 2 - 1)) + 2f) / 2f;
	}

	/** Back in. */
	public static float BackIn(float t)
	{
		return t* t * (2.70158f * t - 1.70158f);
	}

	/** Back out. */
	public static float BackOut(float t)
	{
		return 1f - (--t) * (t) * (-2.70158f * t - 1.70158f);
	}

	/** Back in and out. */
	public static float BackInOut(float t)
	{
		t *= 2f;
		if (t< 1f) return t* t* (2.70158f * t - 1.70158f) / 2f;
		t --;
		return (1f - (--t) * (t) * (-2.70158f * t - 1.70158f)) / 2f + 0.5f;
	}

	// Easing constants.
	private static float B1 = 1f / 2.75f;
	private static float B2 = 2f / 2.75f;
	private static float B3 = 1.5f / 2.75f;
	private static float B4 = 2.5f / 2.75f;
	private static float B5 = 2.25f / 2.75f;
	private static float B6 = 2.625f / 2.75f;
}
