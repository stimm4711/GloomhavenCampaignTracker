package md5e19c8c6d908ac58216f15880adf1d323;


public class CampaignUnlockedScenario
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("GloomhavenCampaignTracker.Business.CampaignUnlockedScenario, Gloomhaven Campaign Tracker", CampaignUnlockedScenario.class, __md_methods);
	}


	public CampaignUnlockedScenario ()
	{
		super ();
		if (getClass () == CampaignUnlockedScenario.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Business.CampaignUnlockedScenario, Gloomhaven Campaign Tracker", "", this, new java.lang.Object[] {  });
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
