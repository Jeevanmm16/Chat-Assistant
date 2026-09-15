namespace ChatAssistantCore.API.Constants;

public static class APIUrlConstants
{
    public const string GetConversations = "api/v1/chat/conversations";
    public const string GetConversation = "api/v1/chat/conversations/{id}";
    public const string CreateConversation = "api/v1/chat/conversations";
    public const string SendMessage = "api/v1/chat/conversations/{id}/messages";
}
