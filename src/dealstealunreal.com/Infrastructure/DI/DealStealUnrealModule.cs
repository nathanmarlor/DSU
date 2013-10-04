namespace dealstealunreal.com.Infrastructure.DI
{
    using System;
    using dealstealunreal.com.Data;
    using dealstealunreal.com.Data.Interfaces;
    using dealstealunreal.com.Infrastructure.Communication;
    using dealstealunreal.com.Infrastructure.Communication.Interfaces;
    using dealstealunreal.com.Infrastructure.Processing;
    using dealstealunreal.com.Infrastructure.Processing.Interfaces;
    using dealstealunreal.com.Infrastructure.Security;
    using dealstealunreal.com.Infrastructure.Security.Interfaces;
    using dealstealunreal.com.Infrastructure.Sessions;
    using dealstealunreal.com.Infrastructure.Sessions.Interfaces;
    using Ninject.Modules;

    public class DealStealUnrealModule : NinjectModule
    {
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
                .WithConstructorArgument("timeout", TimeSpan.FromMinutes(10));

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
        }
    }
}