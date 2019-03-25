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
				System.out.println("���� �����..");
				Socket clientSocket = serverSocket.accept();
				System.out.println("Ŭ���̾�Ʈ ����");
				ClientCount++;
				System.out.println("Ŭ���̾�Ʈ ��:"+serverTest.ClientCount);
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
			in = new BufferedReader(new InputStreamReader(socket.getInputStream()));                                                                                            //bufferstream ���·� in �� ����.
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
				//System.out.println("Ŭ���̾�Ʈ�� ���� ���� ���ڿ�:" + inputLine); //����Ȱ� �ܼ� ���		
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
			System.out.println("Ŭ���̾�Ʈ ��:"+serverTest.ClientCount);
		}
	}
}
 