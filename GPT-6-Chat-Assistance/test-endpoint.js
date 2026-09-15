const url = "https://molabantirupak-5013-resource.services.ai.azure.com/openai/v1/responses";
const apiKey = "Ab4FlPRpbfbkXwLNPUW1yTN5SnRRMdSUA0r7l66FJMjLCf2X5xxPJQQJ99CIACHYHv6XJ3w3AAAAACOGV4pO";

async function test() {
  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      "api-key": apiKey,
    },
    body: JSON.stringify({
      input: [{ role: "user", content: "Hello" }],
      max_tokens: 100,
    }),
  });
  
  console.log("Status:", res.status);
  const data = await res.text();
  console.log("Data:", data);
}

test();
