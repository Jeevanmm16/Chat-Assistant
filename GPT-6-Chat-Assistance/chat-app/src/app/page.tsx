"use client";

import { useState, useRef, useEffect } from "react";
import { useRouter } from "next/navigation";

type Message = {
  id: string;
  role: "user" | "assistant" | "system";
  content: string;
  modelUsed?: string;
  createdAt: string;
};

type Conversation = {
  id: string;
  title: string;
  createdAt: string;
  messages?: Message[];
};

export default function Home() {
  const [conversations, setConversations] = useState<Conversation[]>([]);
  const [currentConversation, setCurrentConversation] = useState<Conversation | null>(null);
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const [username, setUsername] = useState<string | null>(null);
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const router = useRouter();

  // Check auth and fetch all conversations on load
  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) {
      router.push("/login");
      return;
    }
    setUsername(localStorage.getItem("username"));
    fetchConversations(token);
  }, [router]);

  const fetchConversations = async (token: string) => {
    try {
      const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/api/v1/chat/conversations`, {
        headers: { "Authorization": `Bearer ${token}` }
      });
      if (res.status === 401) {
        handleLogout();
        return;
      }
      if (res.ok) {
        const data = await res.json();
        setConversations(data);
      }
    } catch (err) {
      console.error("Failed to fetch conversations", err);
    }
  };

  const loadConversation = async (id: string) => {
    const token = localStorage.getItem("token");
    try {
      const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/api/v1/chat/conversations/${id}`, {
        headers: { "Authorization": `Bearer ${token}` }
      });
      if (res.ok) {
        const data = await res.json();
        setCurrentConversation(data);
        setMessages(data.messages || []);
      }
    } catch (err) {
      console.error("Failed to load conversation", err);
    }
  };

  const createNewChat = async () => {
    const token = localStorage.getItem("token");
    try {
      const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/api/v1/chat/conversations`, {
        method: "POST",
        headers: { "Authorization": `Bearer ${token}` }
      });
      if (res.ok) {
        const data = await res.json();
        setConversations(prev => [data, ...prev]);
        setCurrentConversation(data);
        setMessages([]);
      }
    } catch (err) {
      console.error("Failed to create conversation", err);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("username");
    router.push("/login");
  };

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!input.trim() || isLoading) return;
    
    const token = localStorage.getItem("token");
    if (!token) return handleLogout();

    let convId = currentConversation?.id;

    // Create a new conversation on the fly if none exists
    if (!convId) {
      const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/api/v1/chat/conversations`, { 
        method: "POST",
        headers: { "Authorization": `Bearer ${token}` } 
      });
      if (res.ok) {
        const data = await res.json();
        setConversations(prev => [data, ...prev]);
        setCurrentConversation(data);
        convId = data.id;
      } else {
        return;
      }
    }

    const tempId = Date.now().toString();
    const userMessage: Message = { 
      id: tempId, 
      role: "user", 
      content: input, 
      createdAt: new Date().toISOString() 
    };
    
    setMessages((prev) => [...prev, userMessage]);
    setInput("");
    setIsLoading(true);

    try {
      const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/api/v1/chat/conversations/${convId}/messages`, {
        method: "POST",
        headers: { 
          "Content-Type": "application/json",
          "Authorization": `Bearer ${token}` 
        },
        body: JSON.stringify({ message: userMessage.content }),
      });

      const data = await res.json();

      if (res.ok && data.assistantMessage) {
        setMessages((prev) => [...prev, data.assistantMessage]);
        
        // Refresh conversations list to get updated title if it was a new chat
        if (messages.length === 0) {
            fetchConversations(token);
        }
      } else {
        throw new Error(data.error || "Failed to fetch response");
      }
    } catch (error: any) {
      setMessages((prev) => [
        ...prev,
        {
          id: Date.now().toString(),
          role: "assistant",
          content: `Error: ${error.message || "An unexpected error occurred."}`,
          createdAt: new Date().toISOString()
        },
      ]);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex h-screen bg-gray-900 text-gray-100 font-sans">
      {/* Sidebar */}
      <div className={`${isSidebarOpen ? "w-64" : "w-0"} transition-all duration-300 ease-in-out bg-gray-900 flex flex-col border-r border-gray-700 overflow-hidden shrink-0`}>
        <div className="p-3">
          <button 
            onClick={createNewChat}
            className="w-full flex items-center justify-center space-x-2 bg-gray-800 hover:bg-gray-700 text-white px-4 py-3 rounded-lg transition-colors border border-gray-600"
          >
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" /></svg>
            <span>New Chat</span>
          </button>
        </div>
        
        <div className="flex-1 overflow-y-auto mt-2">
          {conversations.map(c => (
            <button
              key={c.id}
              onClick={() => loadConversation(c.id)}
              className={`w-full text-left px-4 py-3 text-sm truncate transition-colors ${currentConversation?.id === c.id ? 'bg-gray-800 text-white' : 'text-gray-300 hover:bg-gray-800'}`}
            >
              {c.title}
            </button>
          ))}
        </div>
      </div>

      {/* Main Chat Area */}
      <div className="flex-1 flex flex-col h-full bg-gray-800 relative">
        <header className="bg-gray-800 p-4 shadow-sm text-center border-b border-gray-700 flex items-center justify-between">
          <button onClick={() => setIsSidebarOpen(!isSidebarOpen)} className="p-2 hover:bg-gray-700 rounded-md text-gray-400">
            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" /></svg>
          </button>
          <div>
            <h1 className="text-xl font-semibold tracking-wide text-white">Azure AI Chat</h1>
            <p className="text-xs text-gray-400 mt-1">Powered by GPT-6-Astra & GPT-5-mini</p>
          </div>
          <div className="flex items-center space-x-3">
            {username && <span className="text-sm text-gray-400 hidden sm:inline">{username}</span>}
            <button onClick={handleLogout} className="p-2 hover:bg-red-900/50 hover:text-red-400 rounded-md text-gray-400 transition-colors" title="Logout">
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
              </svg>
            </button>
          </div>
        </header>

        <main className="flex-1 overflow-y-auto p-4 md:p-6 space-y-6 scroll-smooth">
          <div className="max-w-3xl mx-auto space-y-6">
            {messages.length === 0 && !isLoading && (
                <div className="h-full flex items-center justify-center text-gray-500 mt-32">
                    <p className="text-lg">How can I help you today?</p>
                </div>
            )}
            
            {messages.map((m, index) => (
              <div
                key={m.id || index}
                className={`flex flex-col ${
                  m.role === "user" ? "items-end" : "items-start"
                }`}
              >
                <div
                  className={`max-w-[85%] px-5 py-3 rounded-2xl text-[15px] leading-relaxed shadow-sm ${
                    m.role === "user"
                      ? "bg-blue-600 text-white rounded-br-none"
                      : "bg-gray-700 text-gray-100 rounded-bl-none border border-gray-600"
                  }`}
                >
                  <div className="whitespace-pre-wrap break-words">{m.content}</div>
                </div>
                {m.modelUsed && m.role === "assistant" && (
                  <span className="text-[11px] text-gray-500 mt-1 ml-2 font-medium">
                    ⚡ Answered by {m.modelUsed}
                  </span>
                )}
              </div>
            ))}
            {isLoading && (
              <div className="flex items-start">
                <div className="max-w-[85%] px-5 py-4 rounded-2xl bg-gray-700 border border-gray-600 rounded-bl-none flex items-center space-x-2">
                  <div className="w-2 h-2 bg-gray-400 rounded-full animate-bounce"></div>
                  <div className="w-2 h-2 bg-gray-400 rounded-full animate-bounce" style={{ animationDelay: "0.2s" }}></div>
                  <div className="w-2 h-2 bg-gray-400 rounded-full animate-bounce" style={{ animationDelay: "0.4s" }}></div>
                </div>
              </div>
            )}
            <div ref={messagesEndRef} />
          </div>
        </main>

        <footer className="bg-gray-800 p-4 border-t border-gray-700">
          <form
            onSubmit={handleSubmit}
            className="max-w-3xl mx-auto flex items-center bg-gray-700 rounded-full border border-gray-600 p-1 focus-within:border-gray-400 focus-within:ring-1 focus-within:ring-gray-400 transition-all shadow-lg"
          >
            <input
              type="text"
              value={input}
              onChange={(e) => setInput(e.target.value)}
              placeholder="Message Azure AI..."
              className="flex-1 bg-transparent text-gray-100 px-5 py-3 outline-none placeholder-gray-400"
              disabled={isLoading}
            />
            <button
              type="submit"
              disabled={isLoading || !input.trim()}
              className="bg-white text-black p-2 md:px-4 md:py-2 rounded-full mr-1 hover:bg-gray-200 transition-colors disabled:opacity-50 disabled:cursor-not-allowed font-medium"
            >
              <span className="hidden md:inline">Send</span>
              <svg
                className="w-5 h-5 md:hidden"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M5 12h14M12 5l7 7-7 7"
                />
              </svg>
            </button>
          </form>
          <p className="text-center text-[10px] text-gray-500 mt-3">
            AI can make mistakes. Verify important information.
          </p>
        </footer>
      </div>
    </div>
  );
}
