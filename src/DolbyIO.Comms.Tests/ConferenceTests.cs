using DolbyIO.Comms;
namespace DolbyIO.Comms.Tests
{
    [Collection("Sdk")]
    public class ConferenceTests
    {
        [Fact]
        public void Test_ConferenceOptions_ShouldMarshall()
        {
            ConferenceOptions src = new ConferenceOptions();
            src.Alias = "TestAlias";
            src.Params.DolbyVoice = true;
            src.Params.Stats = true;
            src.Params.SpatialAudioStyle = SpatialAudioStyle.Shared;

            ConferenceOptions dest;
            NativeTests.ConferenceOptionsTest(src, out dest);

            Assert.NotEqual(src, dest);
            Assert.Equal(src.Alias, dest.Alias);
            Assert.Equal(src.Params.DolbyVoice, dest.Params.DolbyVoice);
            Assert.Equal(src.Params.SpatialAudioStyle, dest.Params.SpatialAudioStyle);
            Assert.Equal(src.Params.Stats, dest.Params.Stats);
        }

        [Fact]
        public void Test_ConferenceInfos_ShouldMarshall()
        {
            ConferenceInfos src = new ConferenceInfos();
            src.Alias = "TestAlias";
            src.Id = "TestId";

            ConferenceInfos dest;
            NativeTests.ConferenceInfosTest(src, out dest);

            Assert.NotEqual(src, dest); // Objects are different
            Assert.Equal(src.Id, dest.Id);
            Assert.Equal(src.Alias, dest.Alias);

            Assert.True(dest.IsNew);
            Assert.Equal(ConferenceStatus.Joining, dest.Status);
            Assert.Equal(2, dest.Permissions.Count);
            Assert.Equal(ConferenceAccessPermissions.Invite, dest.Permissions[0]);
            Assert.Equal(ConferenceAccessPermissions.Join, dest.Permissions[1]);
        }

        [Fact]
        public void Test_JoinOptions_ShouldMarshall()
        {
            JoinOptions src = new JoinOptions();
            src.Connection.ConferenceAccessToken = "access_token";
            src.Connection.SpatialAudio = true;

            src.Constraints.Audio = true;
            src.Constraints.AudioProcessing = true;

            JoinOptions dest;
            NativeTests.JoinOptionsTest(src, out dest);

            Assert.NotEqual(src, dest);
            Assert.Equal(src.Connection.ConferenceAccessToken, dest.Connection.ConferenceAccessToken);
            Assert.Equal(src.Connection.SpatialAudio, dest.Connection.SpatialAudio);
            Assert.Equal(src.Constraints.Audio, dest.Constraints.Audio);
            Assert.Equal(src.Constraints.AudioProcessing, dest.Constraints.AudioProcessing);
        }

        [Fact]
        public void Test_ListenOptions_ShouldMarshall()
        {
            ListenOptions src = new ListenOptions();
            src.Connection.ConferenceAccessToken = "AccessToken";
            src.Connection.SpatialAudio = true;

            ListenOptions dest;
            NativeTests.ListenOptionsTest(src, out dest);

            Assert.NotEqual(src, dest);
            Assert.Equal(src.Connection.ConferenceAccessToken, dest.Connection.ConferenceAccessToken);
            Assert.Equal(src.Connection.SpatialAudio, dest.Connection.SpatialAudio);
        }

        [Fact]
        public void Test_Participant_ShouldMarshall()
        {
            Participant dest;
            NativeTests.ParticipantTest(out dest);

            Assert.Equal("userId", dest.Id);
            Assert.Equal(ParticipantType.User, dest.Type);
            Assert.Equal(ParticipantStatus.Connecting, dest.Status);
            Assert.Equal("Anonymous", dest.Info.Name);
            Assert.Equal("externalId", dest.Info.ExternalId);
            Assert.Equal("http://avatar.url", dest.Info.AvatarURL);
            Assert.True(dest.IsSendingAudio);
            Assert.True(dest.IsAudibleLocally);
        }
    }
}