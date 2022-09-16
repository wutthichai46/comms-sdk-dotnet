using System.Threading.Tasks;

namespace DolbyIO.Comms.Services
{
    /// <summary>
    /// The RemoteAudio service allows the local participant to <see cref="DolbyIO.Comms.Services.RemoteAudioService.MuteAsync(bool, string)">mute</see> selected remote participants and <see cref="DolbyIO.Comms.Services.RemoteAudioService.StopAsync(string)"> stop</see> and <see cref="DolbyIO.Comms.Services.RemoteAudioService.StartAsync(string)">start</see> receiving audio from remote participants in non-Dolby Voice conferences.
    /// </summary>
    /// <example>
    /// <code>
    /// try
    /// {
    ///     await _sdk.Audio.Remote.StartAsync(participantId);
    ///     await _sdk.Audio.Remote.MuteAsync(true, participantId);
    ///     await _sdk.Audio.Remote.StopAsync(participantId);
    /// }
    /// catch
    /// {
    ///     // Error handling
    /// }
    /// </code>
    /// </example>
    public class RemoteAudioService
    {
        /// <summary>
        /// Allows the local participant, who used the
        /// <see cref="DolbyIO.Comms.Services.RemoteAudioService.Stop(string)"> stop</see> method
        /// on a selected remote participant, to start receiving the remote participant's
        /// audio track.
        /// This method allows an audio track from the
        /// desired remote participant to be mixed into the Dolby Voice audio stream
        /// received by the SDK. If the participant does not have their audio enabled,
        /// this method does not enable their audio track.
        /// </summary>
        /// <param name="participantId">The ID of the remote participant whose audio track should be sent to the local participant.</param>
        public async Task StartAsync(string participantId)
        {
            await Task.Run(() => Native.CheckException(Native.StartRemoteAudio(participantId))).ConfigureAwait(false);
        }

        /// <summary>
        /// Allows the local participant to not receive an audio track from
        /// a selected remote participant. This method only impacts the local
        /// participant; the rest of conference participants
        /// will still hear the participant's audio. This method does not allow
        /// the audio track of the selected remote participant to be
        /// mixed into the Dolby Voice audio stream that the SDK receives.
        /// </summary>
        /// <param name="participantId">The ID of the remote participant whose audio track should not be sent to the local participant.</param>
        public async Task StopAsync(string participantId)
        {
            await Task.Run(() => Native.CheckException(Native.StopRemoteAudio(participantId))).ConfigureAwait(false);
        }

        /// <summary>
        /// Stops playing the specified remote participants' audio to the local participant.
        /// The mute method does not notify the server to stop audio stream transmission.
        /// To stop sending an audio stream to the server, use the
        /// <see cref="DolbyIO.Comms.Services.LocalAudioService.Stop">stopAudio</see> method.
        /// </summary>
        /// <remarks>
        /// Attention: This method is only available in non-Dolby Voice conferences.
        /// </remarks>
        /// <param name="muted">A boolean value that indicates the required mute state. True
        /// mutes the remote participant, false un-mutes the remote participant.</param>
        /// <param name="participantId">The ID of the remote participant whose audio should not be played.</param>
        public async Task MuteAsync(bool muted, string participantId)
        {
            await Task.Run(() => Native.CheckException(Native.RemoteMute(muted, participantId))).ConfigureAwait(false);
        }
    }
}