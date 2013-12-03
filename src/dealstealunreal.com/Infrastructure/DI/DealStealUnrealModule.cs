namespace dealstealunreal.com.Infrastructure.DI
{
    using System;
    using Communication;
    using Communication.Interfaces;
    using Data;
    using Data.Interfaces;
    using Ninject.Modules;
    using Processing;
    using Processing.Interfaces;
    using Security;
    using Security.Interfaces;
    using Sessions;
    using Sessions.Interfaces;
    using Utilities;

    /// <summary>
    /// Ninject module
    /// </summary>
    public class DealStealUnrealModule : NinjectModule
    {
        /// <summary>
        /// Load bindings
        /// </summary>
        public override void Load()
        {
            Bind<IEmailSender>()
                .To<EmailSender>();

            Bind<IFacebookAuthenticate>()
                .To<FacebookAuthenticate>();

            Bind<IVoteProcessor>()
                .To<VoteProcessor>();

            Bind<IMemberDataAccess>()
                .To<MemberDataAccess>()
                .InSingletonScope();

            Bind<IDealDataAccess>()
                .To<DealDataAccess>()
                .InSingletonScope();

            Bind<ISessionController>()
                .To<SessionController>()
                .InSingletonScope()
                .WithConstructorArgument("timeout", TimeSpan.FromMinutes(15));

            Bind<IRecoverPassword>()
                .To<RecoverPassword>();

            Bind<ISessionDataAccess>()
                .To<SessionDataAccess>();

            Bind<IHash>()
                .To<Hash>();

            Bind<ICommentDataAccess>()
                .To<CommentDataAccess>();

            Bind<IVoteDataAccess>()
                .To<VoteDataAccess>();

            Bind<ICurrentUser>()
                .To<CurrentUser>();
        }
    }
}