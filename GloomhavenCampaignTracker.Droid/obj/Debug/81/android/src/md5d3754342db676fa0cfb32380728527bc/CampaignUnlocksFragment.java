package md5d3754342db676fa0cfb32380728527bc;


public class CampaignUnlocksFragment
	extends md5aff4aeb270111e744e14f3a0744c9f94.CampaignFragmentBase
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onStop:()V:GetOnStopHandler\n" +
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("GloomhavenCampaignTracker.Droid.Fragments.campaign.unlocks.CampaignUnlocksFragment, Gloomhaven Campaign Tracker", CampaignUnlocksFragment.class, __md_methods);
	}


	public CampaignUnlocksFragment ()
	{
		super ();
		if (getClass () == CampaignUnlocksFragment.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.Fragments.campaign.unlocks.CampaignUnlocksFragment, Gloomhaven Campaign Tracker", "", this, new java.lang.Object[] {  });
	}

	public CampaignUnlocksFragment (android.support.v4.app.FragmentManager p0)
	{
		super ();
		if (getClass () == CampaignUnlocksFragment.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.Fragments.campaign.unlocks.CampaignUnlocksFragment, Gloomhaven Campaign Tracker", "Android.Support.V4.App.FragmentManager, Xamarin.Android.Support.Fragment", this, new java.lang.Object[] { p0 });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onStop ()
	{
		n_onStop ();
	}

	private native void n_onStop ();


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);

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
