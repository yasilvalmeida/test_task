using Xunit;
using SupportCli;
using System;

namespace SupportCliTest;

public class UnitTest
{
  private TicketProcessor ticketProcessor = new TicketProcessor();
  [Fact]
  public void EmptyListTest()
  {
    string expected = "No tickets available";
    string actual = ticketProcessor.List();
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void CreateSuccessTest()
  {
    string expected = "1 has been created";
    string actual = ticketProcessor.Create("create test ticket");
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void CreateFailTest()
  {
    string expected = "Please set the ticket %title%";
    string actual = ticketProcessor.Create("create ");
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void FullListTest()
  {
    ticketProcessor.Create("create test ticket");
    string expected = "Id | Title\n1  | test ticket\n";
    string actual = ticketProcessor.List();
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void ShowWithoutCommentsTest()
  {
    ticketProcessor.Create("create test ticket");
    string expected = "Id=1\nTitle=test ticket\nCurrentState=Open\nAssignedToUser=\nCommentsCount=0\nComments:\n";
    string actual = ticketProcessor.Show("show 1");
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void ShowFailTest()
  {
    ticketProcessor.Create("create test ticket");
    string expected = "Ticket not found for this ID=0";
    string actual = ticketProcessor.Show("show x");
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void AddCommentSuccessTest()
  {
    ticketProcessor.Create("create test ticket");
    string expected = "new comment has been added into 1";
    string actual = ticketProcessor.AddComment("comment 1 test comment");
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void AddCommentWithoutIDFailTest()
  {
    ticketProcessor.Create("create test ticket");
    string expected = "Ticket not found for this ID=0";
    string actual = ticketProcessor.AddComment("comment ");
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void AddCommentWithoutCommentFailTest()
  {
    ticketProcessor.Create("create test ticket");
    string expected = "Please add an not empty comment";
    string actual = ticketProcessor.AddComment("comment 1");
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void ShowWithCommentsTest()
  {
    ticketProcessor.Create("create test ticket");
    ticketProcessor.AddComment("comment 1 test comment");
    string expected = "Id=1\nTitle=test ticket\nCurrentState=Open\nAssignedToUser=\nCommentsCount=1\nComments:\n> test comment\n";
    string actual = ticketProcessor.Show("show 1");
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void AddUserSuccessTest()
  {
    ticketProcessor.Create("create test ticket");
    ticketProcessor.AddComment("comment 1 test comment");
    string expected = "1 has been assigned to John Doe";
    string actual = ticketProcessor.AssignUser("assign 1 John Doe");
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void AddUserFailTest()
  {
    ticketProcessor.Create("create test ticket");
    ticketProcessor.AddComment("comment 1 test comment");
    string expected = "Please add an not empty username";
    string actual = ticketProcessor.AssignUser("assign 1 ");
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void ShowWithCommentsAndUserTest()
  {
    ticketProcessor.Create("create test ticket");
    ticketProcessor.AddComment("comment 1 test comment");
    ticketProcessor.AssignUser("assign 1 John Doe");
    string expected = $"Id=1\nTitle=test ticket\nCurrentState=InProgress\nAssignedToUser=John Doe\nCommentsCount=2\nComments:\n> test comment\n> assigned John Doe {DateTime.UtcNow}\n";
    string actual = ticketProcessor.Show("show 1");
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void CloseSuccessTest()
  {
    ticketProcessor.Create("create test ticket");
    ticketProcessor.AddComment("comment 1 test comment");
    ticketProcessor.AssignUser("assign 1 John Doe");
    string expected = "1 has been closed";
    string actual = ticketProcessor.Close("close 1");
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void CloseFailTest()
  {
    ticketProcessor.Create("create test ticket");
    ticketProcessor.AddComment("comment 1 test comment");
    ticketProcessor.AssignUser("assign 1 John Doe");
    string expected = "Ticket not found for this ID=0";
    string actual = ticketProcessor.Close("close ");
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void ShowWithCommentsAndUserAndCloseTest()
  {
    ticketProcessor.Create("create test ticket");
    ticketProcessor.AddComment("comment 1 test comment");
    ticketProcessor.AssignUser("assign 1 John Doe");
    ticketProcessor.Close("close 1");
    string expected = $"Id=1\nTitle=test ticket\nCurrentState=Closed\nAssignedToUser=John Doe\nCommentsCount=3\nComments:\n> test comment\n> assigned John Doe {DateTime.UtcNow}\n> closed {DateTime.UtcNow}\n";
    string actual = ticketProcessor.Show("show 1");
    Assert.Equal(expected, actual);
  }

  [Fact]
  public void ExitTest()
  {
    string expected = "Exit";
    string actual = ticketProcessor.Exit();
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void WrongCommandTest()
  {
    string expected = "Command not found";
    string actual = ticketProcessor.WrongCommand();
    Assert.Equal(expected, actual);
  }
}