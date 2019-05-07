package md5616b3d30f1150a87250f48a9a23ac5f7;


public abstract class PersonalQuestCounterBase
	extends android.widget.LinearLayout
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("GloomhavenCampaignTracker.Droid.Views.PersonalQuestCounterBase, Gloomhaven Campaign Tracker", PersonalQuestCounterBase.class, __md_methods);
	}


	public PersonalQuestCounterBase (android.content.Context p0)
	{
		super (p0);
		if (getClass () == PersonalQuestCounterBase.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.Views.PersonalQuestCounterBase, Gloomhaven Campaign Tracker", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public PersonalQuestCounterBase (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == PersonalQuestCounterBase.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.Views.PersonalQuestCounterBase, Gloomhaven Campaign Tracker", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public PersonalQuestCounterBase (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == PersonalQuestCounterBase.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.Views.PersonalQuestCounterBase, Gloomhaven Campaign Tracker", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public PersonalQuestCounterBase (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == PersonalQuestCounterBase.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.Views.PersonalQuestCounterBase, Gloomhaven Campaign Tracker", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}

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
