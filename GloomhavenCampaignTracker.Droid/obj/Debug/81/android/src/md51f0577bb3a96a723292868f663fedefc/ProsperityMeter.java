package md51f0577bb3a96a723292868f663fedefc;


public class ProsperityMeter
	extends android.view.View
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDraw:(Landroid/graphics/Canvas;)V:GetOnDraw_Landroid_graphics_Canvas_Handler\n" +
			"n_onMeasure:(II)V:GetOnMeasure_IIHandler\n" +
			"";
		mono.android.Runtime.register ("GloomhavenCampaignTracker.Droid.CustomControls.ProsperityMeter, Gloomhaven Campaign Tracker", ProsperityMeter.class, __md_methods);
	}


	public ProsperityMeter (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ProsperityMeter.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.CustomControls.ProsperityMeter, Gloomhaven Campaign Tracker", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public ProsperityMeter (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ProsperityMeter.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.CustomControls.ProsperityMeter, Gloomhaven Campaign Tracker", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public ProsperityMeter (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == ProsperityMeter.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.CustomControls.ProsperityMeter, Gloomhaven Campaign Tracker", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public ProsperityMeter (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == ProsperityMeter.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.CustomControls.ProsperityMeter, Gloomhaven Campaign Tracker", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onDraw (android.graphics.Canvas p0)
	{
		n_onDraw (p0);
	}

	private native void n_onDraw (android.graphics.Canvas p0);


	public void onMeasure (int p0, int p1)
	{
		n_onMeasure (p0, p1);
	}

	private native void n_onMeasure (int p0, int p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
