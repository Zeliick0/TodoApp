export default class REST {

    public static async sendRequest(url: string, options: RequestInit = {}): Promise<Response> {
        const defaultHeaders: HeadersInit = { "Content-Type": "application/json"};
        const mergedOptions: RequestInit = {
            ...options, headers: { ...defaultHeaders, ...options.headers },
        };

        return await fetch(url,mergedOptions);
    }

    public static async get<T>(url: string): Promise<T> {
       const response = await this.sendRequest(url, { method: "GET" });

       if (!response.ok) {
        throw new Error(`Request failed: ${response.status}`);
       }

       return (await response.json()) as T;
    }

    public static async post<T>(url:  string, body: any): Promise<T> {
        const response = await this.sendRequest(url, { method: "POST", body: JSON.stringify(body)});

        if (!response.ok) {
      throw new Error(`Request failed: ${response.status}`);
        }

        return (await response.json()) as T;
    }

    public static async delete(url: string): Promise<boolean> {
        const response = await this.sendRequest(url, { method: "DELETE"});

        if (!response.ok) {
            throw new Error(`Request failed: ${response.status}`);
        }

        return true;
    }

    public static getWithQuery<T>(url: string, query: Record<string, string>): Promise<T> {
       const params = new URLSearchParams(query).toString();
       const fullUrl = `${url}?${params}`;

       return this.get<T>(fullUrl);
    }
}