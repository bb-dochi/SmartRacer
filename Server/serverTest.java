package serverTest;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
 
public class serverTest {
	public static int ClientCount =0;
	public static void main(String[] args) throws IOException {
		try {
			ServerSocket serverSocket = new ServerSocket(9001);
			while(true){
				System.out.println("서버 대기중..");
				Socket clientSocket = serverSocket.accept();
				System.out.println("클라이언트 연결");
				ClientCount++;
				System.out.println("클라이언트 수:"+serverTest.ClientCount);
				GameThread go = new GameThread(clientSocket);	
				go.start();
			}	
		}catch(Exception e){e.printStackTrace();}
	}
}

class GameThread extends Thread{
	Socket socket = null;
	BufferedReader in = null;
	PrintWriter out = null;
	static List<PrintWriter> list= Collections.synchronizedList(new ArrayList<PrintWriter>());
	static int i=0;
	
	public GameThread(Socket socket){
		this.socket=socket;
		try{
			in = new BufferedReader(new InputStreamReader(socket.getInputStream()));                                                                                            //bufferstream 형태로 in 에 저장.
			out = new PrintWriter(socket.getOutputStream(), true);
			list.add(out);
		}catch(IOException e){
			System.out.println(e.getMessage());
		}
	}
	
	public void run()
	{
		try{
			while (true) {
				String inputLine = null;
				inputLine = in.readLine();
				//System.out.println("클라이언트로 부터 받은 문자열:" + inputLine); //저장된값 콘솔 출력		
				if (inputLine==null)
					break;
				for(PrintWriter out :list){
					out.println(inputLine);
					out.flush();
				}							
			}
			
		}catch(Exception e){
			System.out.println(e.getMessage());
		}finally{
			list.remove(out);
			serverTest.ClientCount --;
			System.out.println("클라이언트 수:"+serverTest.ClientCount);
		}
	}
}
 