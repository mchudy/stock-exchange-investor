ALTER TABLE [dbo].[UserTransaction]
ADD [UserId] int CONSTRAINT [FK_UserTransaction_User] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers]([Id]);