namespace NSWallet.NetStandard.DependencyServices.MediaService.Interfaces
{
	public interface IMediaService
	{
		byte[] ResizeImage(byte[] imageData, float width, float height);
	}
}