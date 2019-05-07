package md5bad2f58a5f1cbbde2050504bdbf61176;


public class CharacterDetailPerks2Fragment
	extends md5bad2f58a5f1cbbde2050504bdbf61176.CharacterDetailFragmentBase
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("GloomhavenCampaignTracker.Droid.Fragments.character.CharacterDetailPerks2Fragment, Gloomhaven Campaign Tracker", CharacterDetailPerks2Fragment.class, __md_methods);
	}


	public CharacterDetailPerks2Fragment ()
	{
		super ();
		if (getClass () == CharacterDetailPerks2Fragment.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.Fragments.character.CharacterDetailPerks2Fragment, Gloomhaven Campaign Tracker", "", this, new java.lang.Object[] {  });
	}


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
